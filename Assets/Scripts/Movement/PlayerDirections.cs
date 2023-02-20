using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirections : MonoBehaviour
{
    //References
    CharacterController controller;
    PlayerMovement playerMovement;

    //Ground
    public Transform groundDirection, fallDirection;
    Vector3 collisionPoint;
    Vector3 forwardDirection;
    RaycastHit groundHit;
    Ray groundRay;
    [HideInInspector] public float slopeAngle, forwardAngle;
    [HideInInspector] public float forwardMult;
    [HideInInspector] public float fallMult;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundDirection();
    }

    //Getting ground direction
    void GroundDirection()
    {
        //Setting forwardDirection to controller position
        forwardDirection = transform.position;

        //Setting forwardDirection based on control input
        if (playerMovement.inputNormalized.magnitude > 0)
            forwardDirection += transform.forward * playerMovement.inputNormalized.y + transform.right * playerMovement.inputNormalized.x;
        else
            forwardDirection += transform.forward;

        //Setting groundDirection to look in the forwardDirection normal
        groundDirection.LookAt(forwardDirection);
        fallDirection.rotation = transform.rotation;

        //Setting ground ray
        groundRay.origin = collisionPoint + Vector3.up * 0.05f;
        groundRay.direction = Vector3.down;

        forwardMult = 1;
        fallMult = 1;

        if (Physics.Raycast(groundRay, out groundHit, 10f))
        {
            //Getting Angles
            slopeAngle = Vector3.Angle(transform.up, groundHit.normal);
            forwardAngle = Vector3.Angle(groundDirection.forward, groundHit.normal) - 90;

            if (forwardAngle < 0 && slopeAngle <= controller.slopeLimit)
            {
                forwardMult = 1 / Mathf.Cos(forwardAngle * Mathf.Deg2Rad);

                //Setting ground direction based on forwardAngle
                groundDirection.eulerAngles += new Vector3(-forwardAngle, 0, 0);
            }
            else if (slopeAngle > controller.slopeLimit)
            {
                fallMult = 1 / Mathf.Cos((slopeAngle) * Mathf.Deg2Rad);

                Vector3 groundCross = Vector3.Cross(groundHit.normal, Vector3.up);
                fallDirection.rotation = Quaternion.FromToRotation(transform.up, Vector3.Cross(groundCross, groundHit.normal));
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        collisionPoint = hit.point;
    }
}
