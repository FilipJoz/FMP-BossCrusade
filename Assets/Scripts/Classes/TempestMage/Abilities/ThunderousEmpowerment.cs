using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThunderousEmpowerment : MonoBehaviour
{
    TempestMage tempestMage;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    [SerializeField] GameObject UiArmorBuff;
    [SerializeField] GameObject vfxPrefab;
    TMP_Text abilityCounter;

    float abilityCd = 5.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    float buffDuration = 5f;
    bool isBuffActive = false;

    // Start is called before the first frame update
    void Start()
    {
        tempestMage = GetComponent<TempestMage>();
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

        if (Input.GetKey(controls.ability6) && abilityTimer <= 0 && !tempestMage.isCasting)
        {
            abilityTimer = abilityCd * tempestMage.CdMultiplier;

            tempestMage.playerMovement.canMove = false;
            tempestMage.anim.SetTrigger("ThunderousEmpowerment");

            ActivateBuff();

            Debug.Log("Thunderous Empowerment used");
        }
        else if (Input.GetKey(controls.ability6) && abilityTimer > 0 && !tempestMage.isCasting)
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
            tempestMage.CdMultiplier = 0.9f;
            StartCoroutine(DeactivateBuff());
            UiBuffActivate();
        }
    }

    IEnumerator DeactivateBuff()
    {
        yield return new WaitForSeconds(buffDuration);
        tempestMage.CdMultiplier = 1f;
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

    void StartAuraVFX()
    {
        StartCoroutine(SpawnAuraVFX());
    }

    IEnumerator SpawnAuraVFX()
    {
        // Instantiate the VFX prefab at the spawn point
        GameObject vfxInstance = Instantiate(vfxPrefab, transform.position, Quaternion.identity);

        // Wait for the VFX to finish playing
        yield return new WaitForSeconds(1);

        // Destroy the VFX instance
        Destroy(vfxInstance.gameObject);
    }
}
