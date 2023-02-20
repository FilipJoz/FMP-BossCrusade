using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarthenSpike : MonoBehaviour
{
    Controls controls;
    Debuff debuff;
    EarthenGuardian earthenGuardian;

    public GameObject ability;
    public GameObject abilityFill;
    TMP_Text abilityCounter;

    float abilityCd = 5.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    float abilityDamage = 10f;


    // Start is called before the first frame update
    void Start()
    {
        controls = GetComponent<Controls>();
        debuff = GetComponent<Debuff>();
        earthenGuardian = GetComponent<EarthenGuardian>();
        abilityCounter = ability.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UseAbility();
        Debugging();
    }

    void UseAbility()
    {
        UiCounter();

        if (Input.GetKey(controls.ability1) && abilityTimer <= 0 && earthenGuardian.inMeleeRange)
        {
            abilityTimer = abilityCd * earthenGuardian.CdMultiplier;

            DealDamage();
            debuff.ApplyDebuff("Earthen Spike Debuff", earthenGuardian.enemy.gameObject);
            ReduceArmor();

            Debug.Log("Earthen Spike used");
        }

        if (Input.GetKey(KeyCode.H))
        {
            debuff.ApplyDebuff("Curse", earthenGuardian.enemy.gameObject);
        }
    }

    void UiCounter()
    {
        abilityTimer -= Time.deltaTime;

        if (abilityTimer <= 0)
        {
            abilityTimerDisplay = 0.0f;
            abilityFill.SetActive(false);
            abilityCounter.gameObject.SetActive(false);
        }
        else
        {
            abilityTimerDisplay = abilityTimer;
            abilityFill.SetActive(true);
            abilityCounter.gameObject.SetActive(true);
        }

        if (abilityTimerDisplay >= 9.9f)
        {
            abilityCounter.text = abilityTimerDisplay.ToString("00");
        }
        else
        {
            abilityCounter.text = abilityTimerDisplay.ToString("0.0");
        }

    }

    void DealDamage()
    {
        earthenGuardian.enemy.health -= Mathf.Round(abilityDamage - (abilityDamage * (earthenGuardian.enemy.armor / (100f + earthenGuardian.enemy.armor))));
    }

    void ReduceArmor()
    {
        int stacks = debuff.GetStacks("Earthen Spike Debuff", earthenGuardian.enemy.gameObject);
        earthenGuardian.enemy.armor = earthenGuardian.enemy.maxArmor - stacks * (earthenGuardian.enemy.maxArmor * 0.05f);
    }

    void Debugging()
    {
        Debug.Log("Earthen Spike Debuff: " + debuff.GetStacks("Earthen Spike Debuff", earthenGuardian.enemy.gameObject));
    }
}
