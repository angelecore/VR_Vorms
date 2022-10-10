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

    public SteamVR_Action_Boolean FirePress = null;

    void Update()
    {
        if (FirePress.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Instantiate(projectilePrefab, barrel.position, barrel.rotation);
            Instantiate(muzzlePrefab, barrel.position, barrel.rotation);
        }
    }
}
