using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private Transform barrel;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private GameObject muzzlePrefab;

    [SerializeField]
    private UnityEvent onFire;

    [SerializeField]
    private AudioClip clip;

    private bool HaveShot = false;

    public SteamVR_Action_Boolean FirePress = null;
    private SwitchPlayers switchingController;

    //Getting the Switching controller dinamically 
    private void Awake()
    {
        switchingController = FindObjectOfType<SwitchPlayers>();
    }

    private void OnEnable()
    {
        HaveShot = false;
    }

    void Update()
    {
        if (FirePress.GetStateDown(SteamVR_Input_Sources.RightHand) && !HaveShot)
        {
            HaveShot = true;
            
            //disabling character switching after firing
            switchingController.IsCharacterSwitchingEnabled = false;
            SoundManager.Instance.PLaySound(clip);
            Instantiate(projectilePrefab, barrel.position, barrel.rotation);
            Instantiate(muzzlePrefab, barrel.position, barrel.rotation);

            //switching the team after shooting
            StartCoroutine(SwitchAfterDelay());
        }
    }

    IEnumerator SwitchAfterDelay()
    {
        yield return new WaitForSeconds(5);

        //switching the team after shooting
        switchingController.SwitchTeam();
    }
}
