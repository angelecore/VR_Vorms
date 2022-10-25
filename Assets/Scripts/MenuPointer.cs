using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPointer : MonoBehaviour
{
    public float DefaultLength = 5.0f;
    public GameObject Dot;
    public MenuVRInpModule InpModule;

    private LineRenderer LineThingy = null;
    private void Awake()
    {
        LineThingy = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        //Use default or distance
        PointerEventData data = InpModule.GetData();
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? DefaultLength : data.pointerCurrentRaycast.distance;

        //Raycast
        RaycastHit hit = CreateRaycast(targetLength);

        //Default end
        Vector3 endPos = transform.position + (transform.forward * targetLength);

        //Update pos if hitting
        if (hit.collider != null)
            endPos = hit.point;

        Dot.transform.position = endPos;

        LineThingy.SetPosition(0, transform.position);
        LineThingy.SetPosition(1, endPos);
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, DefaultLength);

        return hit;
    }
}
