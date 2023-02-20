using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Debuff debuff;

    public float health;
    public float maxHealth = 1000;
    public float armor;
    public float maxArmor = 50;

    // Start is called before the first frame update
    void Start()
    {
        debuff = new Debuff();

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
    }
}
