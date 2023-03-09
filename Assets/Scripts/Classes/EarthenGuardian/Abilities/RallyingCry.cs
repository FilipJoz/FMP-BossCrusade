using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RallyingCry : MonoBehaviour
{
    EarthenGuardian earthenGuardian;
    Stats stats;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    [SerializeField] GameObject UiArmorBuff;
    TMP_Text abilityCounter;

    float abilityCd = 5.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    float buffDuration = 5f;
    float armorIncrease = 0.1f;
    bool isBuffActive = false;

    // Start is called before the first frame update
    void Start()
    {
        earthenGuardian = GetComponent<EarthenGuardian>();
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

        if (Input.GetKey(controls.ability6) && abilityTimer <= 0 && !earthenGuardian.isCasting)
        {
            abilityTimer = abilityCd * earthenGuardian.CdMultiplier;

            ActivateBuff();

            Debug.Log("Rallying Cry used");
        }
        else if (Input.GetKey(controls.ability6) && abilityTimer > 0 && !earthenGuardian.isCasting)
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
            stats.armor *= (1f + armorIncrease);
            StartCoroutine(DeactivateBuff());
            UiBuffActivate();
        }
    }

    IEnumerator DeactivateBuff()
    {
        yield return new WaitForSeconds(buffDuration);
        stats.armor /= (1f + armorIncrease);
        isBuffActive = false;
        UiBuffDeactivate();
    }

    void UiBuffActivate()
    {
        UiArmorBuff.SetActive(true);
    }

    void UiBuffDeactivate()
    {
        UiArmorBuff.SetActive(false);
    }
}
