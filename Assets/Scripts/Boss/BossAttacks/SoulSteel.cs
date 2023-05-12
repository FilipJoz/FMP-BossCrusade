using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Soul Steel Attack", menuName = "Boss Attacks/Soul Steel Attack", order = 7)]
public class SoulSteel : BossAttacks
{
    public float drainRate; // The rate at which health is drained per second
    public float drainDuration; // The duration of the drain effect
    public float pauseTime; // The time at which to pause the animation
    public float resumeTime; // The time at which to resume the animation
    public float ballSpeed;
    public float ballDestroyDistance;

    public GameObject vfxPrefab;

    public override void PerformAttack(Animator animator, Transform bossTransform)
    {
        base.PerformAttack(animator, bossTransform);

        // Find all targets in range and start the drain effect
        Collider[] hitColliders = Physics.OverlapSphere(bossTransform.position, range);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                SoulSteelHandler handler = hitCollider.GetComponent<SoulSteelHandler>();
                if (handler == null)
                {
                    handler = hitCollider.gameObject.AddComponent<SoulSteelHandler>();
                }
                handler.StartDrain(this, bossTransform);
            }
        }

        // Pause the animation after pauseTime seconds
        SoulSteelAnimationHandler animationHandler = animator.GetComponent<SoulSteelAnimationHandler>();
        if (animationHandler == null)
        {
            animationHandler = animator.gameObject.AddComponent<SoulSteelAnimationHandler>();
        }
        animationHandler.StartAnimation(this);
    }
}



