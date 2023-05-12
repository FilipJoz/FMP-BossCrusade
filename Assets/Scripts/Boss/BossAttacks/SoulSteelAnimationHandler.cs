using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSteelAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private SoulSteel attack;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartAnimation(SoulSteel attack)
    {
        this.attack = attack;
        StartCoroutine(PauseAnimation());
    }

    private IEnumerator PauseAnimation()
    {
        yield return new WaitForSeconds(attack.pauseTime);
        animator.speed = 0;
        yield return new WaitForSeconds(attack.drainDuration);
        animator.speed = 1;
    }
}
