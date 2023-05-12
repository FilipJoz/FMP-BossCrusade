using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarthenSpike : MonoBehaviour
{
    Controls controls;
    Debuff debuff;
    EarthenGuardian earthenGuardian;

    [SerializeField] GameObject ability;
    [SerializeField] GameObject abilityFill;
    [SerializeField] GameObject vfxPrefab;
    TMP_Text abilityCounter;

    float abilityCd = 5.0f;
    float abilityTimer = 0.0f;
    float abilityTimerDisplay;

    float abilityDamage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        controls = GetComponent<Controls>();
        debuff = GetComponent<Debuff>();
        earthenGuardian = GetComponent<EarthenGuardian>();
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

        if (Input.GetKey(controls.ability1) && abilityTimer <= 0 && earthenGuardian.inMeleeRange && earthenGuardian.isInfront && !earthenGuardian.isCasting) 
        {
            abilityTimer = abilityCd * earthenGuardian.CdMultiplier;

            earthenGuardian.playerMovement.canMove = false;
            earthenGuardian.anim.SetTrigger("EarthenSpike");

            DealDamage();
            debuff.ApplyDebuff("Earthen Spike Debuff", earthenGuardian.enemy.gameObject);
            ReduceArmor();

            Debug.Log("Earthen Spike used");
        }
        else if (Input.GetKey(controls.ability1) && abilityTimer <= 0 && earthenGuardian.inMeleeRange && !earthenGuardian.isInfront && !earthenGuardian.isCasting)
        {
            Debug.Log("Target is not in front of you");
        }
        else if (Input.GetKey(controls.ability1) && abilityTimer <= 0 && !earthenGuardian.inMeleeRange && !earthenGuardian.isCasting)
        {
            Debug.Log("Target is too far");
        }
        else if (Input.GetKey(controls.ability1) && abilityTimer > 0 && !earthenGuardian.isCasting)
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
        earthenGuardian.enemy.health -= Mathf.Round(abilityDamage - (abilityDamage * (earthenGuardian.enemy.armor / (100f + earthenGuardian.enemy.armor))));
    }

    void ReduceArmor()
    {
        int stacks = debuff.GetStacks("Earthen Spike Debuff", earthenGuardian.enemy.gameObject);
        earthenGuardian.enemy.armor = earthenGuardian.enemy.maxArmor - stacks * (earthenGuardian.enemy.maxArmor * 0.05f);
    }

    void Debugging()
    {
        if (earthenGuardian.enemy != null)
        {
            Debug.Log("Earthen Spike Debuff: " + debuff.GetStacks("Earthen Spike Debuff", earthenGuardian.enemy.gameObject));
        }
    }

    public void StartSpikeVFX()
    {
        StartCoroutine(SpawnSpikeVFX());
    }

    IEnumerator SpawnSpikeVFX()
    {
        // Set the spawn position
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 2; // Increase the y position

        // Set the spawn rotation
        Quaternion spawnRotation = transform.rotation;
        spawnRotation *= Quaternion.Euler(0, 0, 30); // Tilt to the right by 10 degrees

        // Instantiate the VFX prefab at the spawn point with the adjusted position and rotation
        GameObject vfxInstance = Instantiate(vfxPrefab, spawnPosition, spawnRotation);

        // Wait for the VFX to finish playing
        yield return new WaitForSeconds(2);

        // Destroy the VFX instance
        Destroy(vfxInstance.gameObject);
    }

}
