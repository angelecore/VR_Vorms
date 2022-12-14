using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    public AudioClip clip;

    private GameObject m_Pointer;
    public GameObject m_PointerPrefab;
    public SteamVR_Action_Boolean m_TeleportAction;

    private SteamVR_Behaviour_Pose m_Pose = null;
    //private GameObject m_Sphere = null;
    private bool m_hasPosition = false;
    private bool m_isTeleporting = false;
    private float m_FadeTime = 0.5f;

    private float m_movementleft = 10f;

    private float distance = 0f;

    public LayerMask Ground;

    private SwitchPlayers SwitchController;

    private GameObject m_Sphere; // range indicator

    void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Pointer = Instantiate(m_PointerPrefab);
        SwitchController = FindObjectOfType<SwitchPlayers>();
        m_Sphere = SteamVR_Render.Top().origin.parent.GetChild(2).gameObject;
    }

    private void OnEnable()
    {
        if(m_Pointer != null)
            m_Pointer.GetComponent<MeshRenderer>().enabled = true;
        m_movementleft = 10f;
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
        // SoundManager.Instance.PLaySound(clip); the hell
        if (m_TeleportAction.GetStateDown(m_Pose.inputSource))
        {
            if(m_movementleft > 1f)
                m_Sphere.SetActive(true);
        }
        if (m_TeleportAction.GetStateUp(m_Pose.inputSource))
        {
            m_Sphere.SetActive(false);
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
        m_movementleft = m_movementleft - distance;
        Transform Rig = SteamVR_Render.Top().origin.parent;
        SwitchController.IsCharacterSwitchingEnabled = false;
        //this.TryGetComponent<Behaviour>(out Behaviour scriptToDisable);
        //scriptToDisable.enabled = false;
        Vector3 HeadPosition = SteamVR_Render.Top().head.position;

        Vector3 GroundPostion = new Vector3(HeadPosition.x, Rig.position.y, HeadPosition.z);
        Vector3 TeleportPosition = m_Pointer.transform.position - GroundPostion;

        Transform m_Sphere = Rig.GetChild(2);
        m_Sphere.localScale = Vector3.one * m_movementleft*2;
        if(m_movementleft < 1.5f)
            m_Sphere.gameObject.SetActive(false);


        StartCoroutine(MoveRig(Rig, TeleportPosition));
    }

    private IEnumerator MoveRig(Transform Rig, Vector3 translation)
    {

        m_isTeleporting = true;

        SteamVR_Fade.Start(Color.black, m_FadeTime, true);

        yield return new WaitForSeconds(m_FadeTime);

        Rig.position += translation;

        SteamVR_Fade.Start(Color.clear, m_FadeTime, true);

        m_isTeleporting = false;
    }

    private bool UpdatePointer()
    {
        // Ray from controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // hit
        if(Physics.Raycast(ray, out hit, m_movementleft, Ground))
        {
            distance=hit.distance;
            m_Pointer.transform.position = hit.point;
            return true;
        }

        return false;
    }
}
