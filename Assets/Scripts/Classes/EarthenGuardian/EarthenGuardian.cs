using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthenGuardian : MonoBehaviour
{
    public SphereCollider meleeRange;

    Stats stats;
    Debuff debuff;
    EarthenSpike earthenSpike;
    EarthenSmash earthenSmash;
    EarthBarrier earthBarrier;
    Stonewall stonewall;
    public Animator anim;
    public PlayerMovement playerMovement;

    [SerializeField] Slider healthSlider;
    [SerializeField] RectTransform shieldBar;
    float shieldBarWidth;
    float shieldBarPosX;

    public Enemy enemy;
    [SerializeField] GameObject target;
    [SerializeField] float targetHealth;

    [SerializeField] GameObject Castbar;

    [HideInInspector] public float CdMultiplier = 1.0f;

    public bool enemyTarget = true;
    [HideInInspector] public bool inMeleeRange = false;
    [HideInInspector] public bool isInfront = false;
    [HideInInspector] public bool isCasting = false;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        earthenSpike = GetComponent<EarthenSpike>();
        earthenSmash = GetComponent<EarthenSmash>();
        earthBarrier = GetComponent<EarthBarrier>();
        stonewall = GetComponent<Stonewall>();
        debuff = GetComponent<Debuff>();
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

        GetEnemyAngle();
    }

    void HealthCalculation()
    {
        stats.maxHealth = stats.characterMaxHealth + earthBarrier.currentShieldHealth; // + buffs - debuffs
    }

    void UiHealthDisplay()
    {
        healthSlider.value = stats.health / stats.maxHealth;
        if (earthBarrier.currentShieldHealth > 0)
        {
            shieldBar.gameObject.SetActive(true);
            shieldBarWidth = earthBarrier.currentShieldHealth * 0.48f;
            shieldBar.sizeDelta = new Vector2(shieldBarWidth, shieldBar.sizeDelta.y);
            shieldBarPosX = shieldBarWidth / 2;

            Vector3 newAnchPos = shieldBar.anchoredPosition3D;
            newAnchPos.x = shieldBarPosX;
            shieldBar.anchoredPosition3D = newAnchPos;
        }
        else
        {
            shieldBar.gameObject.SetActive(false);
        }


        if (enemyTarget)
        {
            targetHealth = enemy.health / enemy.maxHealth;
            target.GetComponentInChildren<Slider>().value = targetHealth;
        }
    }

    void UiCastbar()
    {
        if (isCasting)
        {
            Castbar.SetActive(true);
            if (earthenSmash.isCastingSmash)
            {
                Castbar.GetComponentInChildren<Slider>().value = earthenSmash.currentCastTime / earthenSmash.castTime;
            }
            else if (stonewall.isCastingStonewall)
            {
                Castbar.GetComponentInChildren<Slider>().value = stonewall.currentCastTime / stonewall.castTime;
            }
        }
        else
        {
            Castbar.SetActive(false);
        }
    }

    void GetEnemyAngle()
    {
        if (enemyTarget && enemy != null)
        {
            Vector3 targetDirection = enemy.transform.position - transform.position;
            float targetAngle = Vector3.Angle(transform.forward, targetDirection);

            isInfront = targetAngle < 60f ? true : false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (enemyTarget) 
        {
            if (collision.gameObject == enemy.gameObject)
            {
                inMeleeRange = true;
            }
        }
        else
        {
            inMeleeRange = false;
        }

    }
    public void OnAttackEnd()
    {
        playerMovement.canMove = true;
    }
}