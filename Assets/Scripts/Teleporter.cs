 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour
{

    private GameObject m_Pointer;
    public GameObject m_PointerPrefab;
    public SteamVR_Action_Boolean m_TeleportAction;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private bool m_hasPosition = false;
    private bool m_isTeleporting = false;
    private float m_FadeTime = 0.5f;

    public LayerMask Ground;

    void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Pointer = Instantiate(m_PointerPrefab);
    }

    private void OnEnable()
    {
        if(m_Pointer != null)
            m_Pointer.GetComponent<MeshRenderer>().enabled = true;
    }
    private void OnDisable()
    {
        if (m_Pointer != null)
            m_Pointer.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Pointer
        m_hasPosition = UpdatePointer();
        m_Pointer.SetActive(m_hasPosition);
        // Teleport
        if (m_TeleportAction.GetStateUp(m_Pose.inputSource)){
            TryTeleport();
        }
    }

    private void TryTeleport()
    {
        // Check for position
        if(!m_hasPosition || m_isTeleporting)
        {
            return;
        }

        Transform cameraRig = SteamVR_Render.Top().origin;
        Vector3 HeadPosition = SteamVR_Render.Top().head.position;

        Vector3 GroundPostion = new Vector3(HeadPosition.x, cameraRig.position.y, HeadPosition.z);
        Vector3 TeleportPosition = m_Pointer.transform.position - GroundPostion;

        StartCoroutine(MoveRig(cameraRig, TeleportPosition));
    }

    private IEnumerator MoveRig(Transform cameraRig, Vector3 translation)
    {

        m_isTeleporting = true;

        SteamVR_Fade.Start(Color.black, m_FadeTime, true);

        yield return new WaitForSeconds(m_FadeTime);

        cameraRig.position += translation;

        SteamVR_Fade.Start(Color.clear, m_FadeTime, true);

        m_isTeleporting = false;
    }

    private bool UpdatePointer()
    {
        // Ray from controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // hit
        if(Physics.Raycast(ray, out hit, 10f,Ground))
        {
            m_Pointer.transform.position = hit.point;
            return true;
        }

        return false;
    }
}
