using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fury Attack", menuName = "Boss Attacks/Fury Attack", order = 5)]
public class Fury : BossAttacks
{
    [SerializeField] BasicAttack basicAttack;
    public float attackSpeed = 2f;
    public float damageIncrease = 1f;
    public float effectDuration = 5f;
    private float timer;

    public void UpdateFury()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                basicAttack.attackDuration += attackSpeed;
                basicAttack.damageMultiplier -= damageIncrease;
            }
        }
    }

    public override void PerformAttack(Animator animator, Transform bossTransform)
    {
        base.PerformAttack(animator, bossTransform);

        basicAttack.attackDuration -= attackSpeed;
        basicAttack.damageMultiplier += damageIncrease;

        timer = effectDuration;
    }
}
