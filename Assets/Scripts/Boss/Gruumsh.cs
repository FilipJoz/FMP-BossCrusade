using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class Gruumsh : MonoBehaviour
{
    [SerializeField] BossAttacks[] bossAttacksP1;
    [SerializeField] BossAttacks[] bossAttacksP2;
    [SerializeField] BossAttacks[] bossAttacksP3;

    Enemy stats;
    Animator animator;
    NavMeshAgent agent;


    [SerializeField] GameObject outerCirclePrefab;
    [SerializeField] GameObject innerCirclePrefab;

    GameObject redCircle;
    GameObject whiteCircle;

    [SerializeField] GameObject outerTrianglePrefab;
    [SerializeField] GameObject innerTrianglePrefab;

    GameObject redTriangle;
    GameObject whiteTriangle;

    bool phase1 = true;
    bool phase2 = false;
    bool phase3 = false;


    [SerializeField] NavMeshSurface surface;

    bool attackInProgress = false;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.health >= 8000 && !attackInProgress)
        {
            phase1 = true;
            phase2 = false;
            phase3 = false;
            StartCoroutine(PerformAttack(bossAttacksP1));
        }
        else if (stats.health < 8000 && stats.health >= 3000 && !attackInProgress)
        {
            phase1 = false;
            phase2 = true;
            phase3 = false;
            StartCoroutine(PerformAttack(bossAttacksP2));
        }
        else if (stats.health < 3000 && !attackInProgress)
        {
            phase1 = false;
            phase2 = false;
            phase3 = true;
            StartCoroutine(PerformAttack(bossAttacksP3));
        }
    }

    IEnumerator PerformAttack(BossAttacks[] attacks)
    {
        foreach (BossAttacks attack in attacks)
        {
            if (phase1 && stats.health < 8000)
            {
                break;
            }
            else if (phase2 && stats.health < 3000)
            {
                break;
            }
            
            
            attackInProgress = true;

            if (attack.requiresTarget)
            {
                // Find a target to attack
                attack.target[0] = FindObjectOfType<EarthenGuardian>().gameObject;
                if (attack.target[0] == null)
                {
                    Debug.LogError("No target found for attack " + attack.attackName);
                    continue;
                }

                float distance = Vector3.Distance(transform.position, attack.target[0].transform.position);
                animator.SetTrigger("Run");

                while (distance > attack.range)
                {
                    // Keep chasing the target
                    agent.SetDestination(attack.target[0].transform.position);
                    yield return null;
                    distance = Vector3.Distance(transform.position, attack.target[0].transform.position);
                }

                animator.ResetTrigger("Run");

                // Smoothly rotate towards the target
                transform.LookAt(attack.target[0].transform);
            }

            if (attack.hasIndicator)
            {
                CreateIndicator(attack);
            }

            agent.isStopped = true;
            // Pass a reference to the boss's transform when calling the PerformAttack method
            attack.PerformAttack(animator, transform);
            yield return new WaitForSeconds(attack.attackDuration);
            agent.isStopped = false;
            attackInProgress = false;
        }
    }

    public void CreateIndicator(BossAttacks attack)
    {
        Vector3 indicatorPosition;
        if (attack.createIndicatorOnTarget)
        {
            // Create the indicator on the target
            indicatorPosition = attack.target[0].transform.position;
        }
        else
        {
            // Create the indicator on the boss
            indicatorPosition = transform.position;
        }

        if (attack.name == "EarthquakeSlam")
        {
            // Calculate the direction from the boss to the target
            Vector3 directionToTarget = (attack.target[0].transform.position - transform.position).normalized;

            // Instantiate the special shape indicator for the special attack
            Quaternion indicatorRotation = Quaternion.LookRotation(directionToTarget) * Quaternion.Euler(-90, 90, 0);
            redTriangle = Instantiate(outerTrianglePrefab, indicatorPosition, indicatorRotation);
            whiteTriangle = Instantiate(innerTrianglePrefab, indicatorPosition, indicatorRotation);

            // Start the coroutine to expand the white circle over time
            StartCoroutine(ExpandTriangleIndicator(attack));
        }
        else
        {
            // Instantiate the circle game objects at the indicator position
            redCircle = Instantiate(outerCirclePrefab, indicatorPosition, Quaternion.Euler(-90, 0, 0));
            whiteCircle = Instantiate(innerCirclePrefab, indicatorPosition, Quaternion.Euler(-90, 0, 0));

            // Set the scale of the red circle to the impact area size
            redCircle.transform.localScale = new Vector3(attack.impactAreaSize, attack.impactAreaSize, 1f);

            // Start the coroutine to expand the white circle over time
            StartCoroutine(ExpandInnerCircle(attack));
        }
    }

    private IEnumerator ExpandInnerCircle(BossAttacks attack)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < attack.timeToImpact)
        {
            // Calculate the current scale of the outer circle based on the elapsed time
            float currentScale = Mathf.Lerp(0f, attack.impactAreaSize, elapsedTime / attack.timeToImpact);
            whiteCircle.transform.localScale = new Vector3(currentScale, currentScale, 1f);

            // Wait for the next frame
            yield return null;

            // Update the elapsed time
            elapsedTime = Time.time - startTime;
        }

        // Destroy the circle game objects
        Destroy(redCircle);
        Destroy(whiteCircle);
    }

    private IEnumerator ExpandTriangleIndicator(BossAttacks attack)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < attack.timeToImpact)
        {
            // Calculate the current scale of the angle indicator based on the elapsed time
            float currentScale = Mathf.Lerp(0f, attack.impactAreaSize, elapsedTime / attack.timeToImpact);
            whiteTriangle.transform.localScale = new Vector3(currentScale, currentScale, 1f);

            // Wait for the next frame
            yield return null;

            // Update the elapsed time
            elapsedTime = Time.time - startTime;
        }

        // Destroy the angle indicator game object
        Destroy(redTriangle);
        Destroy(whiteTriangle);
    }

    void UpdateNavMesh()
    {
        surface.BuildNavMesh();
    }
}


