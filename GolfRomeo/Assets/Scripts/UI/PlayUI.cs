using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    public Text laps;
    public RectTransform mapButtonsParent;

    public GameObject MapButtonPrefab;
    private DirectoryHelper directoryHelper;

    private void Start()
    {
        RaceManager.Instance.TrackNames = new List<string>();
        RaceManager.Instance.Laps = 1;

        directoryHelper = new DirectoryHelper();
        var directories = Directory.GetDirectories(directoryHelper.MapRootFolder);

        foreach (var directory in directories)
        {
            var name = directory.Substring(directoryHelper.MapRootFolder.Length + 1);

            var obj = Instantiate(MapButtonPrefab);
            obj.transform.SetParent(mapButtonsParent, false);
            obj.GetComponent<MapButton>().SetListener(name);
        }
    }

    public void Init()
    {
        gameObject.SetActive(true);
        Start();
    }

    public void AddLaps()
    {
        RaceManager.Instance.Laps++;
        UpdateLapsCount();
    }

    public void DecreaseLaps()
    {
        if (RaceManager.Instance.Laps > 0)
        {
            RaceManager.Instance.Laps--;
            UpdateLapsCount();
        }
    }

    public void StartGame()
    {
        if (RaceManager.Instance.TrackNames.Count > 0)
        {
            RaceManager.Instance.LoadNextRace();
        }
    }

    private void UpdateLapsCount()
    {
        laps.text = RaceManager.Instance.Laps.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Player player = new Player();
            player.Name = "Joystick1";

            RaceManager.Instance.Players.Add(player);
        }
    }

    public void AddAI()
    {
        if (RaceManager.Instance.Players.Count < RaceManager.MaxPlayers)
        {
            Player player = new Player();
            player.Name = "AI";
            player.PlayerType = PlayerType.AI;
        }
    }
}
