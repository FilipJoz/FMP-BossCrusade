using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Controls controls;
    CharacterController controller;
    PlayerDirections directions;
    PlayerMovement playerMovement;

    //Velocity
    Vector3 velocity;
    float gravity = -20, velocityY, terminalVelocity = -25f;

    //Jumping
    bool jumping;
    float jumpSpeed;
    public float jumpHight = 5;
    Vector3 jumpDirection;

    private void Start()
    {
        controls = GetComponent<Controls>();
        controller = GetComponent<CharacterController>();
        directions = GetComponent<PlayerDirections>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        JumpMotion();
    }

    void JumpMotion()
    {
        //Press space to Jump
        if (controller.isGrounded && directions.slopeAngle <= controller.slopeLimit)
            if (Input.GetKey(controls.jump))
            {
                Jump();
            }

        //Apply gravity if not grounded
        if (!controller.isGrounded && velocityY > terminalVelocity)
            velocityY += gravity * Time.deltaTime;
        else if (controller.isGrounded && directions.slopeAngle > controller.slopeLimit)
            velocityY = Mathf.Lerp(velocityY, terminalVelocity, 0.25f);

        //Applying inputs
        if (!jumping)
            velocity = (directions.groundDirection.forward * playerMovement.inputNormalized.magnitude) * (playerMovement.currentSpeed * directions.forwardMult) + directions.fallDirection.up * (velocityY * directions.fallMult);
        else
            velocity = jumpDirection * jumpSpeed + Vector3.up * velocityY;

        //Moving controller
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            //Stop jumping if grounded
            if (jumping)
                jumping = false;

            //Stop gravity if grounded
            velocityY = 0;
        }
    }

    //Jump
    void Jump()
    {
        playerMovement.anim.SetTrigger("Jump");
        //Set jumping to true
        if (!jumping)
            jumping = true;

        //Set jump derection and speed
        jumpDirection = (transform.forward * playerMovement.inputs.y + transform.right * playerMovement.inputs.x).normalized;
        jumpSpeed = playerMovement.currentSpeed/2;
        velocityY = Mathf.Sqrt(-gravity * jumpHight);
    }
}
