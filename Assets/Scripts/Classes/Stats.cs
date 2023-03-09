using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [HideInInspector] public Debuff debuff;
    ActivateDebuffs activateDebuffs;

    public float health;
    public float characterMaxHealth = 500;
    public float maxHealth;
    float healthRegeneration;

    public float armor;
    public float startingArmor = 50;

    // Start is called before the first frame update
    void Start()
    {
        activateDebuffs = GetComponent<ActivateDebuffs>();
        debuff = new Debuff();

        health = characterMaxHealth;
        armor = startingArmor;
    }

    // Update is called once per frame
    void Update()
    {
        if (debuff.GetStacks("Chrono Regen Buff", this.gameObject) == 0)
        {
            activateDebuffs.CancelHoT();
        }
    }
}
