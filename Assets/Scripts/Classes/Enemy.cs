using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Debuff debuff;
    ActivateDebuffs activateDebuffs;

    public float health;
    public float maxHealth = 1000;
    public float armor;
    public float maxArmor = 50;

    // Start is called before the first frame update
    void Start()
    {
        debuff = new Debuff();
        activateDebuffs = GetComponent<ActivateDebuffs>();

        health = maxHealth;
        armor = maxArmor;
    }

    // Update is called once per frame
    void Update()
    {
        if (debuff.GetStacks("Earthen Spike Debuff", this.gameObject) == 0)
        {
            armor = maxArmor;
        }
        
        if (debuff.GetStacks("Thunderous Curse Debuff", this.gameObject) == 0)
        {
            activateDebuffs.CancelDoT();
        }
    }
}
