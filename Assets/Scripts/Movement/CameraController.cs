using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Input Variables
    KeyCode leftMouse = KeyCode.Mouse0, rightMouse = KeyCode.Mouse1, middleMouse = KeyCode.Mouse2;

    //Camera Variables
    public float CameraHight = 0.5f, CameraMaxDistance = 15;
    float CameraMaxTilt = 90;
    [Range(0, 10)]
    public float CameraSpeed = 10;
    float currentPan, currentTilt = 30, currentDistance = 10;

    //CamState
    public CameraState cameraState = CameraState.CameraNone;

    //Collision
    public bool collisionDebug;
    public float collisionCushion = 0.35f;
    float adjustedDistance;
    public LayerMask collisionMask;
    Ray camRay;
    RaycastHit camRayHit;

    //References
    PlayerMovement player;
    public Transform tilt;
    Camera mainCam;

    private void Awake()
    {
        transform.SetParent(null);
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        player.mainCam = this;
        mainCam = Camera.main;

        transform.position = player.transform.position + Vector3.up * CameraHight;
        transform.rotation = player.transform.rotation;

        tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);
        mainCam.transform.position += tilt.forward * -currentDistance;
    }

    void Update()
    {
        if (!Input.GetKey(leftMouse) && !Input.GetKey(rightMouse) && !Input.GetKey(middleMouse)) //If no mouse button is pressed
            cameraState = CameraState.CameraNone;
        else if (Input.GetKey(leftMouse) && !Input.GetKey(rightMouse) && !Input.GetKey(middleMouse)) //If left mouse button is pressed
            cameraState = CameraState.CameraRotate;
        else if (!Input.GetKey(leftMouse) && Input.GetKey(rightMouse) && !Input.GetKey(middleMouse)) //If right mouse button is pressed
            cameraState = CameraState.CameraSteer;
        else if ((Input.GetKey(leftMouse) && Input.GetKey(rightMouse)) || Input.GetKey(middleMouse)) //If left and right mouse button or middle mouse button is pressed
            cameraState = CameraState.CameraRun;

        CameraCollisions();
        CameraInputs();
    }

    private void LateUpdate()
    {
        CameraTransforms();
    }

    void CameraCollisions()
    {
        float camDistance = currentDistance + collisionCushion;

        camRay.origin = transform.position;
        camRay.direction = -tilt.forward;

        if (Physics.Raycast(camRay, out camRayHit, camDistance, collisionMask))
        {
            adjustedDistance = Vector3.Distance(camRay.origin, camRayHit.point) - collisionCushion;
        }
        else
        {
            adjustedDistance = currentDistance;
        }

        if (collisionDebug)
            Debug.DrawLine(camRay.origin, camRay.origin + camRay.direction * camDistance, Color.cyan);
    }

    void CameraInputs()
    {
        if (cameraState != CameraState.CameraNone)
        {
            if (cameraState == CameraState.CameraRotate)
            {
                if (player.steer)
                    player.steer = false;

                currentPan += Input.GetAxis("Mouse X") * CameraSpeed;
            }
            else if (cameraState == CameraState.CameraSteer || cameraState == CameraState.CameraRun)
            {
                if (!player.steer)
                    player.steer = true;
            }

            currentTilt -= Input.GetAxis("Mouse Y") * CameraSpeed;
            currentTilt = Mathf.Clamp(currentTilt, -CameraMaxTilt, CameraMaxTilt);
        }
        else
        {
            if (player.steer)
                player.steer = false;
        }

        player.autoRun = false;

        if (cameraState == CameraState.CameraRun)
        {
            player.autoRun = true;
        }

        currentDistance -= Input.GetAxis("Mouse ScrollWheel") * 5;
        currentDistance = Mathf.Clamp(currentDistance, 0, CameraMaxDistance);
    }

    void CameraTransforms()
    {
        switch (cameraState)
        {
            case CameraState.CameraNone:
            case CameraState.CameraSteer:
            case CameraState.CameraRun:
                currentPan = player.transform.eulerAngles.y;
                break;
            case CameraState.CameraRotate:
                break;
            default:
                break;
        }

        if (cameraState == CameraState.CameraNone)
            currentTilt = 30;

        transform.position = player.transform.position + Vector3.up * CameraHight;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentPan, transform.eulerAngles.z);
        tilt.eulerAngles = new Vector3(currentTilt, tilt.eulerAngles.y, tilt.eulerAngles.z);
        mainCam.transform.position = transform.position + tilt.forward * -adjustedDistance;
    }

    public enum CameraState { CameraNone, CameraRotate, CameraSteer, CameraRun }
}

