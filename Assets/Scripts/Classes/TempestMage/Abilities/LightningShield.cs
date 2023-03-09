using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightningShield : MonoBehaviour
{
    TempestMage tempestMage;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    TMP_Text abilityCounter;

    float abilityCd = 30.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    [HideInInspector] public float currentShieldHealth;
    float bonusShieldHealth = 50f;

    // Start is called before the first frame update
    void Start()
    {
        tempestMage = GetComponent<TempestMage>();
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

        if (Input.GetKey(controls.ability4) && abilityTimer <= 0 && !tempestMage.isCasting)
        {
            abilityTimer = abilityCd * tempestMage.CdMultiplier;

            ActivateShield();

            Debug.Log("Lightning Shield used");
        }
        else if (Input.GetKey(controls.ability4) && abilityTimer > 0 && !tempestMage.isCasting)
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

    void ActivateShield()
    {
        StartCoroutine(ShieldCoroutine());
    }

    IEnumerator ShieldCoroutine()
    {
        currentShieldHealth = bonusShieldHealth;

        yield return new WaitForSeconds(5f);

        currentShieldHealth = 0f;
    }
}
