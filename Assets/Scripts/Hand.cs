using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{

    public SteamVR_Action_Boolean m_GrabAction = null;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private FixedJoint m_Joint = null;

    private Interactable m_CurrentInteractable = null;
    private List<Interactable> m_ContactInteractable = new List<Interactable>();
    // Start is called before the first frame update
    void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GrabAction.GetStateDown(m_Pose.inputSource))
        {
            PickUp();
        }
        if (m_GrabAction.GetStateUp(m_Pose.inputSource))
        {
            Drop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable")){
            return;
        }
        m_ContactInteractable.Add(other.gameObject.GetComponent<Interactable>());

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable")){
            return;
        }
        m_ContactInteractable.Remove(other.gameObject.GetComponent<Interactable>());
    }

    public void PickUp()
    {
        m_CurrentInteractable = GetNearest();

        if (m_CurrentInteractable == null)
            return;

        if (m_CurrentInteractable.hand)
            m_CurrentInteractable.hand.Drop();

        m_CurrentInteractable.transform.position = transform.position;

        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;

        m_CurrentInteractable.hand = this;
    }

    public void Drop()
    {
        if (m_CurrentInteractable == null)
            return;

        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = m_Pose.GetVelocity();
        targetBody.angularVelocity = m_Pose.GetAngularVelocity();

        m_Joint.connectedBody = null;

        m_CurrentInteractable.hand = null;
        m_CurrentInteractable=null;

    }

    private Interactable GetNearest()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Interactable interactable in m_ContactInteractable)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }

        return nearest;
    }
}
