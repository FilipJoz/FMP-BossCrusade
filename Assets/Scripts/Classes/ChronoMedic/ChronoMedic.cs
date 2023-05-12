using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChronoMedic : MonoBehaviour
{
    public SphereCollider range;

    TemporalHealing temporalHealing;
    public Animator anim;
    public PlayerMovement playerMovement;

    [SerializeField] Slider healthSlider;

    public Stats stats;
    public Stats targetStats;
    [SerializeField] GameObject target;
    [SerializeField] float targetHealth;

    [SerializeField] GameObject Castbar;

    [HideInInspector] public float CdMultiplier = 1.0f;

    [HideInInspector] public bool enemyTarget = false;
    [HideInInspector] public bool friendlyTarget = false;

    [HideInInspector] public bool inRange = false;
    [HideInInspector] public bool isInfront = false;
    [HideInInspector] public bool isCasting = false;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        temporalHealing = GetComponent<TemporalHealing>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthCalculation();
        GetTarget();
        UiHealthDisplay();
        UiCastbar();
    }

    void GetTarget()
    {

        targetHealth = target.GetComponentInChildren<Slider>().value;


        GetTargetAngle();
    }

    void HealthCalculation()
    {
        stats.maxHealth = stats.characterMaxHealth; // + buffs - debuffs
    }

    void UiHealthDisplay()
    {
        healthSlider.value = stats.health / stats.characterMaxHealth;

        if (friendlyTarget)
        {
            targetHealth = targetStats.health / targetStats.characterMaxHealth;
            target.GetComponentInChildren<Slider>().value = targetHealth;
        }
    }

    void UiCastbar()
    {
        if (isCasting)
        {
            Castbar.SetActive(true);
            if (temporalHealing.isCastingTH)
            {
                Castbar.GetComponentInChildren<Slider>().value = temporalHealing.currentCastTime / temporalHealing.castTime;
            }
        }
        else
        {
            Castbar.SetActive(false);
        }
    }

    void GetTargetAngle()
    {
        if (friendlyTarget && targetStats != null)
        {
            Vector3 targetDirection = targetStats.transform.position - transform.position;
            float targetAngle = Vector3.Angle(transform.forward, targetDirection);

            isInfront = targetAngle < 60f ? true : false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (friendlyTarget)
        {
            if (collision.gameObject == targetStats.gameObject)
            {
                inRange = true;
            }
        }
        else
        {
            inRange = false;
        }

    }
}
