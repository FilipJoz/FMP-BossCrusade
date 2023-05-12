using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the boulder collided with the target
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Destroy the boulder game object
            Destroy(gameObject);
        }
    }
}
