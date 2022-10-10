using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MovementController : MonoBehaviour
{
    private CharacterController characterController = null;
    private Transform cameraRig = null;
    private Transform head = null;

    public float RotateIncrement = 60;
    public SteamVR_Action_Boolean RotatePressLeft = null;
    public SteamVR_Action_Boolean RotatePressRight = null;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Start()
    {
        cameraRig = SteamVR_Render.Top().origin;
        head = SteamVR_Render.Top().head;
    }

    void Update()
    {
        HandleHead();
        CalculateMovement();
        HandleHeight();
        SnapRotaion();
    }

    private void HandleHead()
    {
        //store current
        Vector3 oldPosition = cameraRig.position;
        Quaternion oldRotation = cameraRig.rotation;

        //rotation
        transform.eulerAngles = new Vector3(0.0f, head.rotation.eulerAngles.y, 0.0f);

        //restore
        cameraRig.position = oldPosition;
        cameraRig.rotation = oldRotation;
    }

    private void CalculateMovement()
    {

    }
    private void HandleHeight()
    {

    }

    private void SnapRotaion()
    {
        float snapValue = 0.0f;

        if (RotatePressLeft.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            snapValue = -Mathf.Abs(RotateIncrement);
        }
        if (RotatePressRight.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            snapValue = Mathf.Abs(RotateIncrement);
        }

        transform.RotateAround(head.position, Vector3.up, snapValue);
    }
}
