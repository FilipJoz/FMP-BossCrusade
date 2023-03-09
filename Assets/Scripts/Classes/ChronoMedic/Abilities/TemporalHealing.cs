using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemporalHealing : MonoBehaviour
{
    Controls controls;
    ChronoMedic chronoMedic;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    TMP_Text abilityCounter;

    float abilityCd = 5.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    float healAmount = 50f;

    [HideInInspector] public float currentCastTime = 0f;
    [HideInInspector] public float castTime = 2f;
    [HideInInspector] public bool isCastingTH;

    // Start is called before the first frame update
    void Start()
    {
        controls = GetComponent<Controls>();
        chronoMedic = GetComponent<ChronoMedic>();
        abilityCounter = ability.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UseAbility();
        CastFinished();
    }

    void UseAbility()
    {
        UiCounter();

        if (Input.GetKey(controls.ability1) && abilityTimer <= 0 && chronoMedic.inRange && chronoMedic.isInfront && !chronoMedic.isCasting)
        {
            abilityTimer = abilityCd * chronoMedic.CdMultiplier;

            StartCasting();

            Debug.Log("Temporal Healing used");
        }
        else if (Input.GetKey(controls.ability1) && abilityTimer <= 0 && chronoMedic.inRange && !chronoMedic.isInfront && !chronoMedic.isCasting)
        {
            Debug.Log("Target is not in front of you");
        }
        else if (Input.GetKey(controls.ability1) && abilityTimer <= 0 && !chronoMedic.inRange && !chronoMedic.isCasting)
        {
            Debug.Log("Target is too far");
        }
        else if (Input.GetKey(controls.ability1) && abilityTimer > 0 && !chronoMedic.isCasting)
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

    void HealTarget()
    {
        chronoMedic.targetStats.health += healAmount;
    }

    public void StartCasting()
    {
        chronoMedic.isCasting = true;
        isCastingTH = true;
    }

    void CastFinished()
    {
        if (isCastingTH)
        {
            currentCastTime += Time.deltaTime;
            if (currentCastTime >= castTime)
            {
                // Cast is complete
                HealTarget();
                chronoMedic.isCasting = false;
                isCastingTH = false;
                currentCastTime = 0f;
            }
        }
    }
}
