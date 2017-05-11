﻿using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayUI : MonoBehaviour
{
    public MenuUI MenuUI;
    public RectTransform ContentParent;
    public GameObject StartButton;

    public Text laps;
    public RectTransform mapButtonsParent;
    public List<PlayerSelectionUI> playerSelections;

    public GameObject MapButtonPrefab;
    private DirectoryHelper directoryHelper;

    private void Start()
    {
        RaceManager.Instance.TrackNames = new List<string>();
        RaceManager.Instance.Laps = 1;

        directoryHelper = new DirectoryHelper();
        var directories = Directory.GetDirectories(directoryHelper.MapRootFolder);

        var mapbuttons = mapButtonsParent.GetComponentsInChildren<Transform>();

        foreach (var buttonTransform in mapbuttons)
        {
            if (buttonTransform.transform.GetInstanceID() != mapButtonsParent.GetInstanceID())
            {
                Destroy(buttonTransform.gameObject);
            }
        }

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
        ContentParent.gameObject.SetActive(true);
        ContentParent.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        ContentParent.GetComponent<RectTransform>().offsetMax = Vector2.zero;

        EventSystem.current.SetSelectedGameObject(StartButton);
        Start();
    }

    public void Back()
    {
        MenuUI.Init();
        ContentParent.gameObject.SetActive(false);
    }

    public void AddLaps()
    {
        RaceManager.Instance.Laps++;
        UpdateLapsCount();
    }

    public void DecreaseLaps()
    {
        if (RaceManager.Instance.Laps > 1)
        {
            RaceManager.Instance.Laps--;
            UpdateLapsCount();
        }
    }

    public void StartGame()
    {
        if (RaceManager.Instance.TrackNames.Count > 0 && RaceManager.Instance.Players.Count > 0)
        {
            RaceManager.Instance.LoadNextRace();
            StartCoroutine(MoveCurtain(new Vector3(0, 1600, 0)));
        }
    }

    IEnumerator MoveCurtain(Vector3 position)
    {
        while (ContentParent.transform.position.y < position.y)
        {
            ContentParent.transform.position += Vector3.up * 6;
            yield return null;
        }

        yield return null;
    }

    private void UpdateLapsCount()
    {
        laps.text = RaceManager.Instance.Laps.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            CreatePlayer("Joystick1", new ControllerScheme().Keyboard());
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            CreatePlayer("Joystick2", new ControllerScheme().Keyboard2());
        }

        foreach (var player in RaceManager.Instance.Players)
        {
            if (player.PlayerType == PlayerType.Player)
            {
                if (Input.GetKeyDown(player.ControllerScheme.Cancel))
                {
                    playerSelections.Find(x => x.IsControllerSchemeSame(player.ControllerScheme)).Leave();
                    RaceManager.Instance.Players.Remove(player);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            AddAI();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void AddAI()
    {
        if (RaceManager.Instance.Players.Count < RaceManager.MaxPlayers)
        {
            Player player = new Player();
            player.Name = "AI";
            player.PlayerType = PlayerType.AI;

            playerSelections[RaceManager.Instance.Players.Count].Join(player);
            RaceManager.Instance.Players.Add(player);
        }
    }

    public Player CreatePlayer(string name, ControllerScheme scheme)
    {
        Player player = new Player();
        player.Name = name;
        player.ControllerScheme = scheme;

        if (playerSelections.Find(x => x.IsControllerSchemeSame(player.ControllerScheme)) == null)
        {
            playerSelections[RaceManager.Instance.Players.Count].Join(player);
            RaceManager.Instance.Players.Add(player);
        }

        return player;
    }
}
