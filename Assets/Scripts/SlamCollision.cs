using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamCollision : MonoBehaviour
{
    public int damage = 10; // The amount of damage to deal to players

    public void DealDamage()
    {
        // Get the collider attached to the earthquake prefab
        Collider collider = GetComponent<Collider>();

        // Check if the collider is colliding with any player game objects
        Collider[] hitColliders = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity);
        foreach (Collider hitCollider in hitColliders)
        {
            // Check if the hit collider has the player tag
            if (hitCollider.CompareTag("Player"))
            {
                Stats stats = hitCollider.GetComponent<Stats>();
                if (stats != null)
                {
                    stats.health -= damage;
                }
            }
        }
    }
}
