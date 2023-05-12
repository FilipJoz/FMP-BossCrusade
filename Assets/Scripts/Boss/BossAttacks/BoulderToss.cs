using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boulder Toss Attack", menuName = "Boss Attacks/Boulder Toss Attack", order = 2)]
public class BoulderToss : BossAttacks
{


    public GameObject boulderPrefab;
    public float boulderThrowForce;
    public float gravity = Physics.gravity.y;

    // Override the PerformAttack method and add a Transform parameter
    public override void PerformAttack(Animator animator, Transform bossTransform)
    {
        base.PerformAttack(animator, bossTransform);

        // Select a random target from the target array
        int randomIndex = Random.Range(0, target.Length);
        GameObject randomTarget = target[randomIndex];

        // Calculate the initial velocity of the boulder
        Vector3 direction = (randomTarget.transform.position - bossTransform.position);
        float yOffset = direction.y;
        direction = new Vector3(direction.x, 0f, direction.z);
        float distance = direction.magnitude;
        float angle = 45f * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(angle);
        distance += yOffset / Mathf.Tan(angle);
        float velocity = Mathf.Sqrt(distance * -gravity / Mathf.Sin(2 * angle));

        // Instantiate the boulder game object at the boss's position
        Vector3 instantiatePosition = new Vector3(bossTransform.position.x, bossTransform.position.y + 5, bossTransform.position.z);
        GameObject boulder = Instantiate(boulderPrefab, instantiatePosition + Vector3.up, Quaternion.identity);
        // Get the rigidbody component of the boulder
        Rigidbody rb = boulder.GetComponent<Rigidbody>();
        // Set the initial velocity of the boulder
        rb.velocity = velocity * direction.normalized;

    }
}