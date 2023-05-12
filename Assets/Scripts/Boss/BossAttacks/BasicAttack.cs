using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Attack", menuName = "Boss Attacks/Basic Attack", order = 1)]
public class BasicAttack : BossAttacks
{
    public float damageMultiplier = 1f;

    public override void PerformAttack(Animator animator, Transform bossTransform)
    {
        base.PerformAttack(animator, bossTransform);

        Debug.Log("BasicAttack");
        target[0].GetComponent<Stats>().health -= damage * damageMultiplier;

    }
}
