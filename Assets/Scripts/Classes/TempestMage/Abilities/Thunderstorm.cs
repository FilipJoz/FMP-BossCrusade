using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Thunderstorm : MonoBehaviour
{
    TempestMage tempestMage;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    TMP_Text abilityCounter;

    float abilityCd = 8.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    float abilityDamage = 20f;
    float radius = 3f;

    float channelTime = 5f;
    float currentChannelTime = 0f;

    Vector3 targetPosition;

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
        EndDamage();
    }

    void UseAbility()
    {
        UiCounter();

        if (Input.GetKey(controls.ability2) && abilityTimer <= 0 && tempestMage.inRange && tempestMage.isInfront && !tempestMage.isCasting)
        {
            abilityTimer = abilityCd * tempestMage.CdMultiplier;

            targetPosition = tempestMage.enemy.gameObject.transform.position;

            SpawnAbility();

            Debug.Log("Thunderstorm used");
        }
        else if (Input.GetKey(controls.ability2) && abilityTimer <= 0 && tempestMage.inRange && !tempestMage.isInfront && !tempestMage.isCasting)
        {
            Debug.Log("Target is not in front of you");
        }
        else if (Input.GetKey(controls.ability2) && abilityTimer <= 0 && !tempestMage.inRange && !tempestMage.isCasting)
        {
            Debug.Log("Target is too far");
        }
        else if (Input.GetKey(controls.ability2) && abilityTimer > 0 && !tempestMage.isCasting)
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

    void SpawnAbility()
    {
        InvokeRepeating("DealDamage", 0, 1);
    }

    void DealDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(targetPosition, radius);

        foreach (Collider col in colliders)
        {
            if (col.tag == "Enemy")
            {
                Enemy enemyHealth = col.GetComponent<Enemy>();
                enemyHealth.health -= abilityDamage;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetPosition, radius);
    }

    void EndDamage()
    {
        if (IsInvoking("DealDamage"))
        {
            currentChannelTime += Time.deltaTime;
        }

        if (currentChannelTime >= channelTime)
        {
            CancelInvoke("DealDamage");
            currentChannelTime = 0f;
        }
    }
}
