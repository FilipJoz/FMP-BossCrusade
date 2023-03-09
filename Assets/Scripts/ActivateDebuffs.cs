using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDebuffs : MonoBehaviour
{
    Enemy enemy;
    Stats stats;

    float thunderousCurseDamage = 5f;
    float chronoRegenHeal = 3f;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoTInvoke()
    {
        if (!IsInvoking("DoT"))
        {
            InvokeRepeating("DoT", 0, 1f);
        }
    }

    public void CancelDoT()
    {
        CancelInvoke("DoT");
    }

    void DoT()
    {
        int stacks = enemy.debuff.GetStacks("Thunderous Curse Debuff", this.gameObject);
        enemy.health -= (stacks * thunderousCurseDamage);
    }
    public void HoTInvoke()
    {
        if (!IsInvoking("HoT"))
        {
            InvokeRepeating("HoT", 0, 1f);
        }
    }

    public void CancelHoT()
    {
        CancelInvoke("HoT");
    }

    void HoT()
    {
        int stacks = stats.debuff.GetStacks("Chrono Regen Buff", this.gameObject);
        stats.health += (stacks * chronoRegenHeal);
    }

}
