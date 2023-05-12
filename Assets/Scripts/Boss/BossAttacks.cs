using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : ScriptableObject
{
    public string attackName;
    public float damage;
    public float range;

    public bool hasIndicator;
    public bool createIndicatorOnTarget;
    public bool requiresTarget;

    public float impactAreaSize;
    public float timeToImpact;
    public float attackDuration;


    public AnimationClip animationClip;
    public GameObject[] target;

    public virtual void PerformAttack(Animator animator, Transform bossTransform)
    {
        animator.SetTrigger(attackName);
    }
}

