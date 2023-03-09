using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThunderousCurse : MonoBehaviour
{
    Controls controls;
    Debuff debuff;
    TempestMage tempestMage;

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
        tempestMage = GetComponent<TempestMage>();
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

        if (Input.GetKey(controls.ability3) && abilityTimer <= 0 && tempestMage.inRange && tempestMage.isInfront && !tempestMage.isCasting)
        {
            abilityTimer = abilityCd * tempestMage.CdMultiplier;

            debuff.ApplyDebuff("Thunderous Curse Debuff", tempestMage.enemy.gameObject);
            tempestMage.enemy.gameObject.GetComponent<ActivateDebuffs>().DoTInvoke();

            Debug.Log("Thunderous Curse used");
        }
        else if (Input.GetKey(controls.ability3) && abilityTimer <= 0 && tempestMage.inRange && !tempestMage.isInfront && !tempestMage.isCasting)
        {
            Debug.Log("Target is not in front of you");
        }
        else if (Input.GetKey(controls.ability3) && abilityTimer <= 0 && !tempestMage.inRange && !tempestMage.isCasting)
        {
            Debug.Log("Target is too far");
        }
        else if (Input.GetKey(controls.ability3) && abilityTimer > 0 && !tempestMage.isCasting)
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
        if (tempestMage.enemy != null)
        {
            Debug.Log("Thunderous Curse Debuff: " + debuff.GetStacks("Thunderous Curse Debuff", tempestMage.enemy.gameObject));
        }
    }
}
