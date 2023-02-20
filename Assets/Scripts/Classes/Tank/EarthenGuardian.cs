using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthenGuardian : MonoBehaviour
{
    public SphereCollider meleeRange;

    Debuff debuff;
    EarthenSpike earthenSpike;
    EarthBarrier earthBarrier;
    Stonewall stonewall;

    public Slider healthSlider;
    public RectTransform shieldBar;
    float shieldBarWidth;
    float shieldBarPosX;

    public Enemy enemy;
    public GameObject target;
    public float targetHealth;

    public GameObject Castbar;

    public float playerHealth;
    public float playerMaxHealth = 500;
    public float maxHealth;
    float healthRegeneration;

    public float playerMana;
    float maxMana = 100f;
    float manaRegeneration;

    public float playerArmor;
    float startingArmor = 50;

    public bool isCasting = false;

    public float CdMultiplier = 1.0f;

    public bool enemyTarget;

    public bool inMeleeRange = false;

    // Start is called before the first frame update
    void Start()
    {
        earthenSpike = GetComponent<EarthenSpike>();
        earthBarrier = GetComponent<EarthBarrier>();
        stonewall = GetComponent<Stonewall>();
        debuff = GetComponent<Debuff>();

        playerHealth = playerMaxHealth;
        playerMana = maxMana;
        playerArmor = startingArmor;
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                string hitObjectTag = hitInfo.collider.tag;
                Debug.Log("Clicked on object with tag: " + hitObjectTag);

                if (hitObjectTag == "Enemy")
                {
                    enemy = hitInfo.collider.GetComponent<Enemy>();
                    targetHealth = target.GetComponentInChildren<Slider>().value;
                    enemyTarget = true;
                    target.gameObject.SetActive(true);
                    List<string> activeDebuffs = debuff.GetActiveDebuffs(enemy.gameObject);
                    foreach (var debuff in activeDebuffs)
                    {
                        Debug.Log("Debuff: " + debuff);
                    }

                    Debug.Log("Active Debuffs: " + string.Join(", ", activeDebuffs));
                }
                else
                {
                    enemyTarget = false;
                    target.gameObject.SetActive(false);
                }
            }
        }
    }

    void HealthCalculation()
    {
        maxHealth = playerMaxHealth + earthBarrier.currentShieldHealth; // + buffs - debuffs
    }

    void UiHealthDisplay()
    {
        healthSlider.value = playerHealth / maxHealth;
        if (earthBarrier.currentShieldHealth > 0)
        {
            shieldBar.gameObject.SetActive(true);
            shieldBarWidth = earthBarrier.currentShieldHealth * 0.395f;
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
            Castbar.GetComponentInChildren<Slider>().value = stonewall.currentCastTime / stonewall.castTime;
        }
        else
        {
            Castbar.SetActive(false);
        }
    }

    void DebugDebuffs()
    {
        foreach (var debuff in Debuff.debuffStacks)
        {
            Debug.Log("Debuff: " + debuff.Key + " has " + debuff.Value + " stacks");
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

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == enemy.gameObject)
        {
            inMeleeRange = false;
        }
    }
}