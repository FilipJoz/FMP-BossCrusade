using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Toxic Eruption Multi Attack", menuName = "Boss Attacks/Toxic Eruption Multi Attack", order = 10)]
public class ToxicEruptionMulti : BossAttacks
{
    public override void PerformAttack(Animator animator, Transform bossTransform)
    {
        base.PerformAttack(animator, bossTransform);

    }
}

