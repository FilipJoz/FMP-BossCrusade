using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Earthquake Slam Attack", menuName = "Boss Attacks/Earthquake Slam Attack", order = 3)]
public class EarthquakeSlam : BossAttacks
{
    public GameObject earthquakePrefab;
    public float delay = 1f; 

    public override void PerformAttack(Animator animator, Transform bossTransform)
    {
        base.PerformAttack(animator, bossTransform);

        Quaternion spawnRotation = Quaternion.Euler(bossTransform.eulerAngles.x, bossTransform.eulerAngles.y + 135f, bossTransform.eulerAngles.z);

        // Create an instance of the earthquake prefab
        GameObject earthquake = Instantiate(earthquakePrefab, bossTransform.position, spawnRotation);

        // Call the DealDamage method after the specified delay
        earthquake.GetComponent<SlamCollision>().Invoke("DealDamage", delay);
    }
}
