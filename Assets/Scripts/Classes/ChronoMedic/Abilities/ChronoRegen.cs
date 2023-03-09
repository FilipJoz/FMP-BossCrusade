using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChronoRegen : MonoBehaviour
{
    Controls controls;
    Debuff debuff;
    ChronoMedic chronoMedic;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    TMP_Text abilityCounter;

    float abilityCd = 5.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    // Start is called before the first frame update
    void Start()
    {
        controls = GetComponent<Controls>();
        debuff = GetComponent<Debuff>();
        chronoMedic = GetComponent<ChronoMedic>();
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

        if (Input.GetKey(controls.ability2) && abilityTimer <= 0 && chronoMedic.inRange && chronoMedic.isInfront && !chronoMedic.isCasting)
        {
            abilityTimer = abilityCd * chronoMedic.CdMultiplier;

            debuff.ApplyDebuff("Chrono Regen Buff", chronoMedic.targetStats.gameObject);
            chronoMedic.targetStats.gameObject.GetComponent<ActivateDebuffs>().HoTInvoke();

            Debug.Log("Chrono Regen used");
        }
        else if (Input.GetKey(controls.ability2) && abilityTimer <= 0 && chronoMedic.inRange && !chronoMedic.isInfront && !chronoMedic.isCasting)
        {
            Debug.Log("Target is not in front of you");
        }
        else if (Input.GetKey(controls.ability2) && abilityTimer <= 0 && !chronoMedic.inRange && !chronoMedic.isCasting)
        {
            Debug.Log("Target is too far");
        }
        else if (Input.GetKey(controls.ability2) && abilityTimer > 0 && !chronoMedic.isCasting)
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

    void Debugging()
    {
        if (chronoMedic.targetStats != null)
        {
            Debug.Log("Chrono Regen Buff: " + debuff.GetStacks("Chrono Regen Buff", chronoMedic.targetStats.gameObject));
        }
    }
}
