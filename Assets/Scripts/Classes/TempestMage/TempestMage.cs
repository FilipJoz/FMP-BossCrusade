using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempestMage : MonoBehaviour
{
    public SphereCollider range;

    Stats stats;
    Debuff debuff;
    LightningShield lightningShield;
    LightningBolt lightningBolt;
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
    [HideInInspector] public bool inRange = false;
    [HideInInspector] public bool isInfront = false;
    [HideInInspector] public bool isCasting = false;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        debuff = GetComponent<Debuff>();
        lightningShield = GetComponent<LightningShield>();
        lightningBolt = GetComponent<LightningBolt>();
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
        stats.maxHealth = stats.characterMaxHealth + lightningShield.currentShieldHealth; // + buffs - debuffs
    }

    void UiHealthDisplay()
    {
        healthSlider.value = stats.health / stats.maxHealth;
        if (lightningShield.currentShieldHealth > 0)
        {
            shieldBar.gameObject.SetActive(true);
            shieldBarWidth = lightningShield.currentShieldHealth * 1.14f;
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
        }
    }

    void UiCastbar()
    {
        if (isCasting)
        {
            Castbar.SetActive(true);
            if (lightningBolt.isCastingLB)
            {
                Castbar.GetComponentInChildren<Slider>().value = lightningBolt.currentCastTime / lightningBolt.castTime;
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
                inRange = true;
            }
        }
        else
        {
            inRange = false;
        }

    }

    public void OnAttackEnd()
    {
        playerMovement.canMove = true;
    }
}
