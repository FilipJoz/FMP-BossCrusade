using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stonewall : MonoBehaviour
{
    EarthenGuardian earthenGuardian;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    TMP_Text abilityCounter;

    [SerializeField] GameObject shieldPrefab;
    [HideInInspector] public float castTime = 3f;
    [HideInInspector] public float currentCastTime = 0f;
    Vector3 initialPosition;

    float abilityCd = 30.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    [HideInInspector] public bool isCastingStonewall = false;

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

        if (Input.GetKey(controls.ability5) && abilityTimer <= 0 && !earthenGuardian.isCasting)
        {
            abilityTimer = abilityCd * earthenGuardian.CdMultiplier;

            earthenGuardian.playerMovement.canMove = false;
            earthenGuardian.anim.SetTrigger("Stonewall");

            StartCasting();

            Debug.Log("Stonewall used");
        }
        else if (Input.GetKey(controls.ability5) && abilityTimer > 0 && !earthenGuardian.isCasting)
        {
            Debug.Log("Ability is not ready yet");
        }
    }

    public void StartCasting()
    {
        initialPosition = transform.position;
        earthenGuardian.isCasting = true;
        isCastingStonewall = true;
    }

    void SpawnShield()
    {
        // Spawn the shield in front of the character
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        Vector3 spawnPosition = position + transform.forward * 5f;
        GameObject shield = Instantiate(shieldPrefab, spawnPosition, transform.rotation);
        // Set the duration of the shield
        Destroy(shield, 7f);
    }

    void CastFinished()
    {
        if (isCastingStonewall)
        {
            currentCastTime += Time.deltaTime;
            if (currentCastTime >= castTime)
            {
                // Cast is complete
                SpawnShield();
                earthenGuardian.isCasting = false;
                isCastingStonewall = false;
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
