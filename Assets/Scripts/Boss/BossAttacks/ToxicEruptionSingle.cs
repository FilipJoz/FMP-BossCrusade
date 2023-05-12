using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Toxic Eruption Single Attack", menuName = "Boss Attacks/Toxic Eruption Single Attack", order = 9)]
public class ToxicEruptionSingle : BossAttacks
{
    public override void PerformAttack(Animator animator, Transform bossTransform)
    {
        base.PerformAttack(animator, bossTransform);

    }
}
