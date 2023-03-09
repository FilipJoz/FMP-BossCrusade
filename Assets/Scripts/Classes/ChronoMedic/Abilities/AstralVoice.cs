using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AstralVoice : MonoBehaviour
{
    ChronoMedic chronoMedic;
    Stats stats;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    [SerializeField] GameObject UiHealthBuff;
    TMP_Text abilityCounter;

    float abilityCd = 5.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    float buffDuration = 5f;
    float healthIncrease = 0.1f;
    bool isBuffActive = false;

    // Start is called before the first frame update
    void Start()
    {
        chronoMedic = GetComponent<ChronoMedic>();
        stats = GetComponent<Stats>();
        controls = GetComponent<Controls>();
        abilityCounter = ability.GetComponentInChildren<TMP_Text>();

        UiBuffDeactivate();
    }

    // Update is called once per frame
    void Update()
    {
        UseAbility();
    }

    void UseAbility()
    {
        UiCounter();

        if (Input.GetKey(controls.ability6) && abilityTimer <= 0 && !chronoMedic.isCasting)
        {
            abilityTimer = abilityCd * chronoMedic.CdMultiplier;

            ActivateBuff();

            Debug.Log("Rallying Cry used");
        }
        else if (Input.GetKey(controls.ability6) && abilityTimer > 0 && !chronoMedic.isCasting)
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

    void ActivateBuff()
    {
        if (!isBuffActive)
        {
            isBuffActive = true;
            stats.characterMaxHealth *= (1f + healthIncrease);
            StartCoroutine(DeactivateBuff());
            UiBuffActivate();
        }
    }

    IEnumerator DeactivateBuff()
    {
        yield return new WaitForSeconds(buffDuration);
        stats.characterMaxHealth /= (1f + healthIncrease);
        isBuffActive = false;
        UiBuffDeactivate();
    }

    void UiBuffActivate()
    {
        UiHealthBuff.SetActive(true);
    }

    void UiBuffDeactivate()
    {
        UiHealthBuff.SetActive(false);
    }
}
