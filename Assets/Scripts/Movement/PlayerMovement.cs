using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Inputs
    Controls controls;
    [HideInInspector] public Vector2 inputs, inputNormalized;
    [HideInInspector] public bool steer;
    [HideInInspector] public bool autoRun = false;

    //Running
    [HideInInspector] public float currentSpeed;
    public float runSpeed = 3, rotateSpeed = 25;

    //Rotating
    float rotation;
    Vector3 characterRotation;

    //References
    CharacterController controller;
    PlayerDirections directions;
    [HideInInspector] public CameraController mainCam;
    public Animator anim;
    public bool canMove = true;

    void Start()
    {
        controls = GetComponent<Controls>();
        controller = GetComponent<CharacterController>();
        directions = GetComponent<PlayerDirections>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove)
        {
            GetInputs();
            Locomotion();
        }
        else
        {
            inputs.x = 0;
            anim.SetFloat("X", 0);
            inputs.y = 0;
            anim.SetFloat("Y", 0);
            rotation = 0;
            Locomotion();
        }

    }

    //Motions
    void Locomotion()
    {

        //Running and walking
        if (controller.isGrounded && directions.slopeAngle <= controller.slopeLimit)
        {
            inputNormalized = inputs;

            currentSpeed = runSpeed;

            if (inputNormalized.y < 0)
                currentSpeed = currentSpeed / 2;
        }
        else if (!controller.isGrounded || directions.slopeAngle > controller.slopeLimit)
        {
            inputNormalized = Vector2.Lerp(inputNormalized, Vector2.zero, 0.025f);
            currentSpeed = Mathf.Lerp(currentSpeed, 0, 0.025f);
        }

        //Rotating
        characterRotation = transform.eulerAngles + new Vector3(0, rotation * rotateSpeed * Time.deltaTime, 0);
        transform.eulerAngles = characterRotation;
    }

    //Inputs
    void GetInputs()
    {
        //Forwards
        if (Input.GetKey(controls.forwards))
        {
            inputs.y = 1;
            anim.SetFloat("Y", 1);
        }

        //Backwards
        if (Input.GetKey(controls.backwards))
        {
            if (Input.GetKey(controls.forwards))
            {
                inputs.y = 0;
                anim.SetFloat("Y", 0);
            }
            else
            {
                inputs.y = -1;
                anim.SetFloat("Y", -1);
            }
        }

        //FW Nothing
        if (!Input.GetKey(controls.forwards) && !Input.GetKey(controls.backwards))
        {
            inputs.y = 0;
            anim.SetFloat("Y", 0);
        }

        if (autoRun)
        {
            inputs.y += 1;

            inputs.y = Mathf.Clamp(inputs.y, -1, 1);
        }

        //Strafe Left
        if (Input.GetKey(controls.strafeRight))
        {
            inputs.x = 1;
            anim.SetFloat("X", 1);
        }

        //Strafe Right
        if (Input.GetKey(controls.strafeLeft))
        {
            if (Input.GetKey(controls.strafeRight))
            {
                inputs.x = 0;
                anim.SetFloat("X", 0);
            }
            else
            {
                inputs.x = -1;
                anim.SetFloat("X", -1);
            }
        }

        //Strafe LR Nothing
        if (!Input.GetKey(controls.strafeRight) && !Input.GetKey(controls.strafeLeft))
        {
            inputs.x = 0;
            anim.SetFloat("X", 0);
        }

        if (steer)
        {
            //Rotate Left
            if (Input.GetKey(controls.rotateRight))
            {
                inputs.x = 1;
                anim.SetFloat("X", 1);
            }

            //Rotate Right
            if (Input.GetKey(controls.rotateLeft))
            {
                if (Input.GetKey(controls.rotateRight))
                {
                    inputs.x = 0;
                    anim.SetFloat("X", 0);
                }
                else
                {
                    inputs.x = -1;
                    anim.SetFloat("X", -1);
                }
            }

            //Rotate LR Nothing
            if (!Input.GetKey(controls.rotateRight) && !Input.GetKey(controls.rotateLeft))
            {
                inputs.x = 0;
                anim.SetFloat("X", 0);
            }

            inputs.x = Mathf.Clamp(inputs.x, -1, 1);
        }

        if (steer)
            rotation = Input.GetAxis("Mouse X") * mainCam.CameraSpeed;
        else
        {
            //Rotate Left
            if (Input.GetKey(controls.rotateRight))
                rotation = 1;

            //Rotate Right
            if (Input.GetKey(controls.rotateLeft))
            {
                if (Input.GetKey(controls.rotateRight))
                    rotation = 0;
                else
                    rotation = -1;
            }

            //Rotate LR Nothing
            if (!Input.GetKey(controls.rotateRight) && !Input.GetKey(controls.rotateLeft))
                rotation = 0;
        }
    }
}

