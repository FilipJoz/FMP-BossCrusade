using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightningBolt : MonoBehaviour
{
    TempestMage tempestMage;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    [SerializeField] GameObject projectilePrefab;
    TMP_Text abilityCounter;

    float abilityCd = 1.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    [HideInInspector] public float currentCastTime = 0f;
    [HideInInspector] public float castTime = 2f;

    [HideInInspector] public bool isCastingLB = false;

    // Start is called before the first frame update
    void Start()
    {
        tempestMage = GetComponent<TempestMage>();
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

        if (Input.GetKey(controls.ability1) && abilityTimer <= 0 && tempestMage.inRange && tempestMage.isInfront && !tempestMage.isCasting)
        {
            abilityTimer = abilityCd * tempestMage.CdMultiplier;

            tempestMage.playerMovement.canMove = false;
            tempestMage.anim.SetTrigger("LightningBolt");
            StartCasting();

            Debug.Log("Lightning Bolt used");
        }
        else if (Input.GetKey(controls.ability1) && abilityTimer <= 0 && tempestMage.inRange && !tempestMage.isInfront && !tempestMage.isCasting)
        {
            Debug.Log("Target is not in front of you");
        }
        else if (Input.GetKey(controls.ability1) && abilityTimer <= 0 && !tempestMage.inRange && !tempestMage.isCasting)
        {
            Debug.Log("Target is too far");
        }
        else if (Input.GetKey(controls.ability1) && abilityTimer > 0 && !tempestMage.isCasting)
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
    public void StartCasting()
    {
        tempestMage.isCasting = true;
        isCastingLB = true;
    }

    void CastFinished()
    {
        if (isCastingLB)
        {
            currentCastTime += Time.deltaTime;
            if (currentCastTime >= castTime)
            {
                // Cast is complete
                SpawnProjectile();
                tempestMage.isCasting = false;
                isCastingLB = false;
                currentCastTime = 0f;
            }
        }
    }
    void SpawnProjectile ()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        LightningBoltController projectileController = projectile.GetComponent<LightningBoltController>();
        projectileController.target = tempestMage.enemy.gameObject;
    }
}
