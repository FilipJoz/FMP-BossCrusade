using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WindWalk : MonoBehaviour
{
    TempestMage tempestMage;
    PlayerMovement playerMovement;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    [SerializeField] GameObject VFXPrefab;
    TMP_Text abilityCounter;

    float abilityCd = 5.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    // Start is called before the first frame update
    void Start()
    {
        tempestMage = GetComponent<TempestMage>();
        playerMovement = GetComponent<PlayerMovement>();
        controls = GetComponent<Controls>();
        abilityCounter = ability.GetComponentInChildren<TMP_Text>();

        VFXPrefab.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UseAbility();
    }

    void UseAbility()
    {
        UiCounter();

        if (Input.GetKey(controls.ability5) && abilityTimer <= 0 && !tempestMage.isCasting)
        {
            abilityTimer = abilityCd * tempestMage.CdMultiplier;

            ActivateSpeed();

            Debug.Log("Wind Walk used");
        }
        else if (Input.GetKey(controls.ability5) && abilityTimer > 0 && !tempestMage.isCasting)
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

    void ActivateSpeed()
    {
        StartCoroutine(SpeedCoroutine());
    }

    IEnumerator SpeedCoroutine()
    {
        playerMovement.runSpeed = 20;
        VFXPrefab.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        playerMovement.runSpeed = 10;
        VFXPrefab.gameObject.SetActive(false);
    }
}
