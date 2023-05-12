using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSteelHandler : MonoBehaviour
{
    private GameObject vfxInstance;
    private SoulSteel attack;
    private Transform bossTransform;
    private bool draining;
    float elapsedTime = 0;

    private List<GameObject> balls = new List<GameObject>();

    public void StartDrain(SoulSteel attack, Transform bossTransform)
    {
        this.attack = attack;
        this.bossTransform = bossTransform;
        draining = true;

        StartCoroutine(SpawnBalls());
        StartCoroutine(DrainHealth());
    }

    private IEnumerator DrainHealth()
    {

        while (elapsedTime < attack.drainDuration && draining)
        {
            // Drain health from target
            GetComponent<Stats>().health -= attack.drainRate * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        draining = false;

        // Destroy the VFX
        Destroy(vfxInstance);
    }

    private IEnumerator SpawnBalls()
    {
        while (draining)
        {
            // Instantiate the VFX and set its position
            GameObject ball = Instantiate(attack.vfxPrefab);
            ball.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            balls.Add(ball);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Update()
    {
        // Stop draining if out of range
        if (draining && Vector3.Distance(bossTransform.position, transform.position) > attack.range)
        {
            draining = false;
        }

        // Start draining if within range and not already draining
        if (!draining && elapsedTime < attack.drainDuration && Vector3.Distance(bossTransform.position, transform.position) <= attack.range)
        {
            draining = true;
            StartCoroutine(SpawnBalls());
            StartCoroutine(DrainHealth());
        }

        // Move balls towards boss and destroy them when they reach the boss
        for (int i = balls.Count - 1; i >= 0; i--)
        {
            GameObject ball = balls[i];
            Vector3 targetPosition = new Vector3(bossTransform.position.x, bossTransform.position.y + 6f, bossTransform.position.z);
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, targetPosition, attack.ballSpeed * Time.deltaTime);
            if (Vector3.Distance(ball.transform.position, targetPosition) < attack.ballDestroyDistance)
            {
                Destroy(ball);
                balls.RemoveAt(i);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(bossTransform.position, attack.range);
    }
}
