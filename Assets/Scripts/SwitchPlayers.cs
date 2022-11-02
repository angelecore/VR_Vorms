using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SwitchPlayers : MonoBehaviour
{
    public CameraRigTeamNumber[] PlayerList;
    public SteamVR_Action_Boolean SwitchTeamsAction;
    public SteamVR_Action_Boolean SwitchCharaktersAction;
    private int CurrentTeam = 0;
    private int CurrentChar = 0;
    private int Maxteam1 = 0;
    private int Maxteam2 = 0;
    CameraRigTeamNumber[,] teams;
    void Start()
    {
        PlayerList = FindObjectsOfType(typeof(CameraRigTeamNumber)) as CameraRigTeamNumber[];
        CameraRigTeamNumber[,] temp = new CameraRigTeamNumber[2, PlayerList.Length];
        int team1MaxCount = 0;
        int team2MaxCount = 0;

        foreach (CameraRigTeamNumber item in PlayerList)
        {
            if (item.team == 1)
            {
                temp[0, team1MaxCount] = item;
                team1MaxCount++;
            }
            else
            {
                temp[1, team2MaxCount] = item;
                team2MaxCount++;
            }
        }
        Maxteam1 = team1MaxCount;
        Maxteam2 = team2MaxCount;
        teams = temp;
    }

    // Update is called once per frame
    void Update()
    {
        if (SwitchTeamsAction.GetStateDown(SteamVR_Input_Sources.RightHand) || SwitchCharaktersAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            // sita reiks ideti kai galesim sunaukit kitus zaidejus.
            /*PlayerList = FindObjectsOfType(typeof(CameraRigTeamNumber)) as CameraRigTeamNumber[];
            CameraRigTeamNumber[,] teams = new CameraRigTeamNumber[2,PlayerList.Length];
            int team1MaxCount = 0;
            int team2MaxCount = 0;
            
            foreach (CameraRigTeamNumber item in PlayerList)
            {
                if (item.team == 1)
                {
                    teams[0, team1MaxCount] = item;
                    team1MaxCount++;
                }
                else
                { 
                    teams[1, team2MaxCount] = item;
                    team2MaxCount++;
                }
            }
            Maxteam1 = team1MaxCount;
            Maxteam2 = team2MaxCount;*/


            if (SwitchTeamsAction.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                //Debug.Log("teamswitch");
                teams[CurrentTeam, CurrentChar].transform.GetChild(0).gameObject.SetActive(false);
                if (CurrentTeam == 0)
                    CurrentTeam = 1;
                else
                    CurrentTeam = 0;
                CurrentChar = 0;
                teams[CurrentTeam, CurrentChar].transform.GetChild(0).gameObject.SetActive(true);
            }

            if (SwitchCharaktersAction.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                //Debug.Log("charSwitch");
                teams[CurrentTeam, CurrentChar].transform.GetChild(0).gameObject.SetActive(false);
                CurrentChar++;
                if ((CurrentTeam == 0 && CurrentChar >= Maxteam1) || (CurrentTeam == 1 && CurrentChar >= Maxteam2))
                    CurrentChar = 0;
                teams[CurrentTeam, CurrentChar].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
