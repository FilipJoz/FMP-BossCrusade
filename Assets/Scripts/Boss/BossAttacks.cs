using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAttacks", menuName = "Boss Attacks", order = 1)]
public class BossAttacks : ScriptableObject
{
    public string attackName;
    public float damage;
    public float range;
    public float attackDuration;
    public AnimationClip animationClip;
    public GameObject[] target;

    public void PerformAttack()
    {
        switch (attackName)
        {
            case "BasicAttack":

                break;
            case "BoulderToss":

                break;
            case "EarthquakeSlam":

                break;
            case "EnhancedEarthquakeSlam":

                break;
            case "Fury":

                break;
            case "Quake":

                break;
            case "SoulSteel":

                break;
            case "Stomp":

                break;
            case "ToxicEruptionMulti":

                break;
            case "ToxicEruption":

                break;
            default:
                break;
        }
    }
}

