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
    public GameObject OrangePlayerPrefab;
    public GameObject BluePlayerPrefab;
    public int TeamPlayerAmount = 3;
    private int CurrentTeam = 0;
    private int CurrentCharInd = 0;
    private CameraRigTeamNumber CurrentChar;
    private int Maxteam1 = 0;
    private int Maxteam2 = 0;
    public bool CanSwitchTeammates = true;

    private bool IsGameOver = false;

    public bool IsCharacterSwitchingEnabled = true;
    //Need to set this ^ to false after teleporting (the same way ProjectileController.cs does it)

    CameraRigTeamNumber[,] teams;
    void Start()
    {
        CurrentChar = FindObjectOfType<CameraRigTeamNumber>();

        SpawnPlayers();

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
        //CurrentChar = teams[CurrentTeam, CurrentCharInd];
    }

    // Update is called once per frame
    void Update()
    {
        if (SwitchCharaktersAction.GetStateDown(SteamVR_Input_Sources.RightHand) && IsCharacterSwitchingEnabled)
        {
            //Disabling current character's rotation script and Camera rig, enabling its Model
            CurrentChar.TryGetComponent<Behaviour>(out Behaviour scriptToDisable);
            scriptToDisable.enabled = false;
            CurrentChar.transform.GetChild(0).gameObject.SetActive(false);
            CurrentChar.transform.GetChild(1).gameObject.SetActive(true);
            CurrentChar.transform.GetChild(3).gameObject.SetActive(true);

            CurrentCharInd++;
            if ((CurrentTeam == 0 && CurrentCharInd >= Maxteam1) || (CurrentTeam == 1 && CurrentCharInd >= Maxteam2))
            {
                CurrentCharInd = 0;
            }
            CurrentChar = teams[CurrentTeam, CurrentCharInd];

            //Enabling new character's rotation script and Camera rig, disabling its Model
            CurrentChar.transform.GetChild(0).gameObject.SetActive(true);
            CurrentChar.transform.GetChild(1).gameObject.SetActive(false);
            CurrentChar.transform.GetChild(2).localScale = Vector3.one *20;
            CurrentChar.transform.GetChild(3).gameObject.SetActive(false);
            CurrentChar.TryGetComponent<Behaviour>(out Behaviour scriptToEnable);
            scriptToEnable.enabled = true;
        }
    }

    public void SwitchTeam()
    {
        // Code to execute after the delay
        UpdateTeamsInfo();

        //Disabling current character's rotation script and Camera rig, enabling its Model
        CurrentChar.TryGetComponent<Behaviour>(out Behaviour scriptToDisable);
        scriptToDisable.enabled = false;
        CurrentChar.transform.GetChild(0).gameObject.SetActive(false);
        CurrentChar.transform.GetChild(1).gameObject.SetActive(true);

        if (IsGameOver)
        {
            CurrentTeam = 2;
            IsCharacterSwitchingEnabled = false;
            StartCoroutine(ReturnToMenu());
        }
        else
        {
            if (CurrentTeam == 0)
            {
                CurrentTeam = 1;
                foreach (CameraRigTeamNumber item in PlayerList)
                {
                    Debug.Log("yes");
                    if (!item.alive)
                        continue;
                    if(item.team == 1)
                    {
                        Debug.Log("yes yes");
                        item.transform.GetChild(3).gameObject.SetActive(false);
                    }
                    if (item.team == 2)
                    {
                        Debug.Log("yes yes");
                        item.transform.GetChild(3).gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                CurrentTeam = 0;
                foreach (CameraRigTeamNumber item in PlayerList)
                {
                    Debug.Log("yes2");
                    if (!item.alive)
                        continue;
                    if (item.team == 2)
                    {
                        item.transform.GetChild(3).gameObject.SetActive(false);
                    }
                    if (item.team == 1)
                    {
                        item.transform.GetChild(3).gameObject.SetActive(true);
                    }
                }
            }
        }
        CurrentCharInd = 0;
        CurrentChar = teams[CurrentTeam, CurrentCharInd];

        //Enabling new character's rotation script and Camera rig, disabling its Model
        CurrentChar.transform.GetChild(0).gameObject.SetActive(true);
        CurrentChar.transform.GetChild(1).gameObject.SetActive(false);
        CurrentChar.transform.GetChild(2).localScale = Vector3.one * 20;
        CurrentChar.transform.GetChild(3).gameObject.SetActive(false);
        CurrentChar.TryGetComponent<Behaviour>(out Behaviour scriptToEnable);
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

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(7);

        Scenes.LoadPreviousScene();
    }

    private void SpawnPlayers()
    {
        List<GameObject> spawnPoints = new List<GameObject>();
        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("Spawnpoint"));

        if(spawnPoints.Count < TeamPlayerAmount * 2 - 1)
        {
            throw new System.Exception("Not enough spawnpoints to spawn the desired amount of players");
        }

        for (int i = 0; i < TeamPlayerAmount; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Count);
            Instantiate(BluePlayerPrefab, spawnPoints[spawnPointIndex].transform.position, Quaternion.identity);
            spawnPoints.RemoveAt(spawnPointIndex);
        }

        for (int i = 0; i < (TeamPlayerAmount - 1); i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Count);
            Instantiate(OrangePlayerPrefab, spawnPoints[spawnPointIndex].transform.position, Quaternion.identity);
            spawnPoints.RemoveAt(spawnPointIndex);
        }
    }
    
    public CameraRigTeamNumber GetCurrentPlayer()
    {
        return CurrentChar;
    }
}
