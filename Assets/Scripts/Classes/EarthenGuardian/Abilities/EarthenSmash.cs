using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarthenSmash : MonoBehaviour
{
    EarthenGuardian earthenGuardian;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    [SerializeField] GameObject vfxPrefab;
    TMP_Text abilityCounter;

    float abilityCd = 8.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    float abilityDamage = 50f;
    float radius = 2f;

    [HideInInspector] public float currentCastTime = 0f;
    [HideInInspector] public float castTime = 1.5f;

    [HideInInspector] public bool isCastingSmash = false;

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

        if (Input.GetKey(controls.ability2) && abilityTimer <= 0 && earthenGuardian.inMeleeRange && earthenGuardian.isInfront && !earthenGuardian.isCasting)
        {
            abilityTimer = abilityCd * earthenGuardian.CdMultiplier;

            earthenGuardian.playerMovement.canMove = false;
            earthenGuardian.anim.SetTrigger("EarthenSmash");
            StartCasting();

            Debug.Log("Earthen Smash used");
        }
        else if (Input.GetKey(controls.ability2) && abilityTimer <= 0 && earthenGuardian.inMeleeRange && !earthenGuardian.isInfront && !earthenGuardian.isCasting)
        {
            Debug.Log("Target is not in front of you");
        }
        else if (Input.GetKey(controls.ability2) && abilityTimer <= 0 && !earthenGuardian.inMeleeRange && !earthenGuardian.isCasting)
        {
            Debug.Log("Target is too far");
        }
        else if (Input.GetKey(controls.ability2) && abilityTimer > 0 && !earthenGuardian.isCasting)
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
        earthenGuardian.isCasting = true;
        isCastingSmash = true;
    }

    void CastFinished()
    {
        if (isCastingSmash)
        {
            currentCastTime += Time.deltaTime;
            if (currentCastTime >= castTime)
            {
                // Cast is complete
                DealDamage();
                earthenGuardian.isCasting = false;
                isCastingSmash = false;
                currentCastTime = 0f;
            }
        }
    }
    void DealDamage()
    {
        earthenGuardian.enemy.health -= Mathf.Round(abilityDamage - (abilityDamage * (earthenGuardian.enemy.armor / (100f + earthenGuardian.enemy.armor))));

        Collider[] colliders = Physics.OverlapSphere(earthenGuardian.enemy.transform.position, radius);

        int enemiesDamaged = 0;

        foreach (Collider col in colliders)
        {
            if (col.tag == "Enemy")
            {
                if (col.gameObject == earthenGuardian.enemy.gameObject)
                {
                    continue;
                }

                Enemy enemyHealth = col.GetComponent<Enemy>();
                enemyHealth.health -= Mathf.Round(abilityDamage - (abilityDamage * (enemyHealth.armor / (100f + enemyHealth.armor))));

                enemiesDamaged++;

                if (enemiesDamaged >= 2)
                {
                    break;
                }
            }
        }
    }

    public void StartSlashVFX()
    {
        StartCoroutine(SpawnSlashVFX());
    }

    IEnumerator SpawnSlashVFX()
    {
        // Set the spawn position
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 2; // Increase the y position

        // Instantiate the VFX prefab at the spawn point
        GameObject vfxInstance = Instantiate(vfxPrefab, spawnPosition, transform.rotation);

        // Wait for the VFX to finish playing
        yield return new WaitForSeconds(2);

        // Destroy the VFX instance
        Destroy(vfxInstance.gameObject);
    }
}
