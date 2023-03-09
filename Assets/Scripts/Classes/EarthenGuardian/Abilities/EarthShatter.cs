using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarthShatter : MonoBehaviour
{
    EarthenGuardian earthenGuardian;
    Controls controls;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    TMP_Text abilityCounter;

    float abilityCd = 8.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    float abilityDamage = 50f;
    float radius = 3f;


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
    }

    void UseAbility()
    {
        UiCounter();

        if (Input.GetKey(controls.ability3) && abilityTimer <= 0 && !earthenGuardian.isCasting)
        {
            abilityTimer = abilityCd * earthenGuardian.CdMultiplier;

            DealDamage();

            Debug.Log("Earthen Shatter used");
        }
        else if (Input.GetKey(controls.ability3) && abilityTimer > 0 && !earthenGuardian.isCasting)
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

    void DealDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
