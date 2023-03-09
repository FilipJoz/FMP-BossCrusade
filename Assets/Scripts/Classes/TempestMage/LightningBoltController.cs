using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBoltController : MonoBehaviour
{
    public GameObject target; 
    public Transform targetLocation;
    public float speed = 10f; 
    public float lifetime = 10f; 
    public float damage = 10f; 

    private float currentLifetime = 0f; 

    void Update()
    {
        targetLocation = target.transform;

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetLocation.position, speed * Time.deltaTime);

        // Rotate towards the target
        transform.LookAt(targetLocation);

        // Increase the current lifetime of the projectile
        currentLifetime += Time.deltaTime;

        // If the projectile has exceeded its lifetime, destroy it
        if (currentLifetime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            Enemy enemyHealth = other.GetComponent<Enemy>();
            enemyHealth.health -= damage;
            Destroy(gameObject);
        }
    }
}
