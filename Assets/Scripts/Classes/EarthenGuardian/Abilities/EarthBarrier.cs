using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarthBarrier : MonoBehaviour
{
    EarthenGuardian earthenGuardian;
    Stats stats;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    TMP_Text abilityCounter;

    float abilityCd = 16.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    [HideInInspector] public float currentShieldHealth;
    float bonusShieldHealth;
    float maxShieldHealth;

    // Start is called before the first frame update
    void Start()
    {
        earthenGuardian = GetComponent<EarthenGuardian>();
        stats = GetComponent<Stats>();
        controls = GetComponent<Controls>();
        abilityCounter = ability.GetComponentInChildren<TMP_Text>();

        currentShieldHealth = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        UseAbility();
    }

    void UseAbility()
    {
        UiCounter();

        if (Input.GetKey(controls.ability4) && abilityTimer <= 0 && !earthenGuardian.isCasting)
        {
            abilityTimer = abilityCd * earthenGuardian.CdMultiplier;

            ShieldCalculation();
            AddShield();

            Debug.Log("Earthen Barrier used");
        }
        else if (Input.GetKey(controls.ability4) && abilityTimer > 0 && !earthenGuardian.isCasting)
        {
            Debug.Log("Ability is not ready yet");
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

    void ShieldCalculation()
    {
        bonusShieldHealth = stats.characterMaxHealth * 0.12f;
        maxShieldHealth = stats.characterMaxHealth * 0.2f;
    }

    void AddShield()
    {
        currentShieldHealth += bonusShieldHealth;

        if (currentShieldHealth >= maxShieldHealth)
        {
            currentShieldHealth = maxShieldHealth;
        }
    }
}
