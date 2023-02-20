using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stonewall : MonoBehaviour
{
    EarthenGuardian earthenGuardian;
    Controls controls;

    public GameObject ability;
    public GameObject abilityFill;
    TMP_Text abilityCounter;

    public float castTime = 2f;
    public GameObject shieldPrefab;
    public float currentCastTime = 0f;
    private Vector3 initialPosition;

    float abilityCd = 30.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;


    // Start is called before the first frame update
    void Start()
    {
        earthenGuardian = GetComponent<EarthenGuardian>();
        controls = GetComponent<Controls>();
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

        if (Input.GetKey(controls.ability5) && abilityTimer <= 0)
        {
            abilityTimer = abilityCd * earthenGuardian.CdMultiplier;

            StartCasting();

            Debug.Log("Stonewall used");
        }
    }

    public void StartCasting()
    {
        initialPosition = transform.position;
        earthenGuardian.isCasting = true;
    }

    void SpawnShield()
    {
        // Spawn the shield in front of the character
        Vector3 spawnPosition = transform.position + transform.forward * 2f;
        GameObject shield = Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);
        // Set the duration of the shield
        Destroy(shield, 7f);
    }

    void CastFinished()
    {
        if (earthenGuardian.isCasting)
        {
            currentCastTime += Time.deltaTime;
            if (currentCastTime >= castTime)
            {
                // Cast is complete
                SpawnShield();
                earthenGuardian.isCasting = false;
                currentCastTime = 0f;
            }
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
}
