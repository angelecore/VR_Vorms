using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    public CameraRigTeamNumber Parent;

    public void DoDie()
    {
        Parent.alive = false;
        Parent.transform.GetChild(3).gameObject.SetActive(false);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
