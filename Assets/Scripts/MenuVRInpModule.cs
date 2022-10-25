using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class MenuVRInpModule : BaseInputModule
{
    public Camera PointerCamera;
    public SteamVR_Input_Sources TargetSource;
    public SteamVR_Action_Boolean ClickAction;

    private GameObject CurrentObject = null;
    private PointerEventData Data = null;

    protected override void Awake()
    {
        base.Awake();

        Data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        //Reset data, set camera
        Data.Reset();
        Data.position = new Vector2(PointerCamera.pixelWidth / 2, PointerCamera.pixelHeight / 2);

        //Raycast
        eventSystem.RaycastAll(Data, m_RaycastResultCache);
        Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        CurrentObject = Data.pointerCurrentRaycast.gameObject;

        //Clear raycast
        m_RaycastResultCache.Clear();

        //Handle hovering
        HandlePointerExitAndEnter(Data, CurrentObject);

        //Press
        if (ClickAction.GetStateDown(TargetSource))
            ProcessPress(Data);

        //Release
        if (ClickAction.GetStateUp(TargetSource))
            ProcessRelease(Data);
    }

    public PointerEventData GetData()
    {
        return Data;
    }

    private void ProcessPress(PointerEventData data)
    {
        //Set raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        //Get obj if hitting
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(CurrentObject, data, ExecuteEvents.pointerDownHandler);

        //If no down handler, try get click handler
        if (newPointerPress == null)
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(CurrentObject);

        //Set data
        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = CurrentObject;
    }

    private void ProcessRelease(PointerEventData data)
    {
        //Execute pointer up
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        //Check for click handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(CurrentObject);

        //Check if clicked on an object
        if(data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        //Clear selected gameobject
        eventSystem.SetSelectedGameObject(null);

        //Reset the data
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }
}
