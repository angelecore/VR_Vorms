using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class SwitchPlayers : MonoBehaviour
{
    public CameraRigTeamNumber[] PlayerList;
    public SteamVR_Action_Boolean SwitchTeamsAction;
    public SteamVR_Action_Boolean SwitchCharaktersAction;
    public Text VictoryText;
    private int CurrentTeam = 0;
    private int CurrentChar = 0;
    private int Maxteam1 = 0;
    private int Maxteam2 = 0;

    private bool IsGameOver = false;
    public bool IsTeamSwitchingEnabled = true;

    public bool IsCharacterSwitchingEnabled = true;
    //Need to disable this ^ after teleporting (the same way ProjectileController.cs does it)

    CameraRigTeamNumber[,] teams;
    void Start()
    {
        PlayerList = FindObjectsOfType(typeof(CameraRigTeamNumber)) as CameraRigTeamNumber[];
        CameraRigTeamNumber[,] temp = new CameraRigTeamNumber[3, PlayerList.Length];
        int team1MaxCount = 0;
        int team2MaxCount = 0;

        foreach (CameraRigTeamNumber item in PlayerList)
        {
            if (!item.alive)
                continue;

            if (item.team == 1)
            {
                temp[0, team1MaxCount] = item;
                team1MaxCount++;
            }
            else if (item.team == 2)
            {
                temp[1, team2MaxCount] = item;
                team2MaxCount++;
            }
            else
            {
                temp[2, 0] = item;
            }
        }
        Maxteam1 = team1MaxCount;
        Maxteam2 = team2MaxCount;
        teams = temp;
    }

    // Update is called once per frame
    void Update()
    {
        if (SwitchCharaktersAction.GetStateDown(SteamVR_Input_Sources.RightHand) && IsCharacterSwitchingEnabled)
        {
            //Disabling current character's rotation script and Camera rig, enabling its Model
            teams[CurrentTeam, CurrentChar].TryGetComponent<Behaviour>(out Behaviour scriptToDisable);
            scriptToDisable.enabled = false;
            teams[CurrentTeam, CurrentChar].transform.GetChild(0).gameObject.SetActive(false);
            teams[CurrentTeam, CurrentChar].transform.GetChild(1).gameObject.SetActive(true);

            CurrentChar++;
            if ((CurrentTeam == 0 && CurrentChar >= Maxteam1) || (CurrentTeam == 1 && CurrentChar >= Maxteam2))
                CurrentChar = 0;

            //Enabling new character's rotation script and Camera rig, disabling its Model
            teams[CurrentTeam, CurrentChar].transform.GetChild(0).gameObject.SetActive(true);
            teams[CurrentTeam, CurrentChar].transform.GetChild(1).gameObject.SetActive(false);
            teams[CurrentTeam, CurrentChar].TryGetComponent<Behaviour>(out Behaviour scriptToEnable);
            scriptToEnable.enabled = true;
        }
    }

    public void SwitchTeam()
    {
        // Code to execute after the delay
        UpdateTeamsInfo();

        //Disabling current character's rotation script and Camera rig, enabling its Model
        teams[CurrentTeam, CurrentChar].TryGetComponent<Behaviour>(out Behaviour scriptToDisable);
        scriptToDisable.enabled = false;
        teams[CurrentTeam, CurrentChar].transform.GetChild(0).gameObject.SetActive(false);
        teams[CurrentTeam, CurrentChar].transform.GetChild(1).gameObject.SetActive(true);

        if (IsGameOver)
        {
            CurrentTeam = 2;
            IsTeamSwitchingEnabled = false;
            IsCharacterSwitchingEnabled = false;
        }
        else
        {
            if (CurrentTeam == 0)
                CurrentTeam = 1;
            else
                CurrentTeam = 0;
        }
        CurrentChar = 0;

        //Enabling new character's rotation script and Camera rig, disabling its Model
        teams[CurrentTeam, CurrentChar].transform.GetChild(0).gameObject.SetActive(true);
        teams[CurrentTeam, CurrentChar].transform.GetChild(1).gameObject.SetActive(false);
        teams[CurrentTeam, CurrentChar].TryGetComponent<Behaviour>(out Behaviour scriptToEnable);
        scriptToEnable.enabled = true;

        IsCharacterSwitchingEnabled = true;
    }

    public void UpdateTeamsInfo()
    {
        // sita reiks ideti kai galesim sunaukit kitus zaidejus.
        PlayerList = FindObjectsOfType(typeof(CameraRigTeamNumber)) as CameraRigTeamNumber[];

        teams = new CameraRigTeamNumber[3, PlayerList.Length];
        int team1MaxCount = 0;
        int team2MaxCount = 0;

        foreach (CameraRigTeamNumber item in PlayerList)
        {
            if (!item.alive)
                continue;

            if (item.team == 1)
            {
                teams[0, team1MaxCount] = item;
                team1MaxCount++;
            }
            else if (item.team == 2)
            {
                teams[1, team2MaxCount] = item;
                team2MaxCount++;
            }
            else
            {
                teams[2, 0] = item;
            }
        }
        Maxteam1 = team1MaxCount;
        Maxteam2 = team2MaxCount;

        if (Maxteam1 == 0)
        {
            VictoryText.text = "Blue team Won!";
            ColorUtility.TryParseHtmlString("#3A70A4", out Color parsedColor);
            VictoryText.color = parsedColor;
            IsGameOver = true;
        }
        if (Maxteam2 == 0)
        {
            VictoryText.text = "Orange team Won!";
            ColorUtility.TryParseHtmlString("#E79A2A", out Color parsedColor);
            VictoryText.color = parsedColor;
            IsGameOver = true;
        }
    }
}
