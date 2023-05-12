using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stomp Attack", menuName = "Boss Attacks/Stomp Attack", order = 8)]
public class Stomp : BossAttacks
{
    public float castTime;

    public override void PerformAttack(Animator animator, Transform bossTransform)
    {
        base.PerformAttack(animator, bossTransform);
        CoroutineRunner.Instance.StartCoroutine(CastStomp(bossTransform));
    }

    private IEnumerator CastStomp(Transform bossTransform)
    {
        // Wait for the cast time to finish
         yield return new WaitForSeconds(timeToImpact);
        // Perform the stomp attack
        CoroutineRunner.Instance.StartCoroutine(PerformStomp(bossTransform));
    }

    private IEnumerator PerformStomp(Transform bossTransform)
    {
        yield return new WaitForSeconds(0f);
        // Get all colliders within the impact area
        var colliders = Physics.OverlapSphere(bossTransform.position, impactAreaSize*2);
        var damagedEnemies = new HashSet<Stats>();
        foreach (var collider in colliders)
        {
            // Check if the collider is an enemy and deal damage
            var enemy = collider.GetComponent<Stats>();
            if (enemy != null && !damagedEnemies.Contains(enemy))
            {
                enemy.health -= damage;
                damagedEnemies.Add(enemy);
            }
        }
    }
}

