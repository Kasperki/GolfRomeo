﻿using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayUI : MonoBehaviour
{
    public MenuUI MenuUI;
    public RaceOptionsUI RaceOptionsUI;
    public RectTransform ContentParent;
    public GameObject StartButton;

    public Text laps, AICountLabel;
    public RectTransform mapButtonsParent;
    public List<PlayerSelectionUI> playerSelections;

    public GameObject MapButtonPrefab;

    public Button[] ButtonsToMapSelectionNavigation;

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

        for (int i = 0; i < directories.Length; i++)
        {
            var directory = directories[i];
            var name = directory.Substring(directoryHelper.MapRootFolder.Length + 1);

            var obj = Instantiate(MapButtonPrefab);
            obj.transform.SetParent(mapButtonsParent, false);
            obj.GetComponent<MapSelectionButton>().SetListener(name);

            if (i == 0)
            {
                foreach (var btn in ButtonsToMapSelectionNavigation)
                {
                    var navigation = btn.navigation;
                    navigation.selectOnDown = obj.GetComponent<Button>();
                    btn.navigation = navigation;
                }
            }
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

        RaceOptionsUI.Init();
    }

    public void Back()
    {
        MenuUI.Init();
        ContentParent.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        if (RaceManager.Instance.TrackNames.Count > 0 && RaceManager.Instance.Players.Count > 0)
        {
            RaceManager.Instance.StartNewTournament();
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

    void Update()
    {
        if (GameManager.CheckState(State.Menu))
        {
            if (Input.GetKeyDown(new ControllerScheme().Keyboard().Submit))
            {
                CreatePlayer("WASD", new ControllerScheme().Keyboard());
            }

            if (Input.GetKeyDown(new ControllerScheme().Keyboard2().Submit))
            {
                CreatePlayer("ARROWS", new ControllerScheme().Keyboard2());
            }

            var playersToRemove = new List<Player>();
            for (int i = 0; i < RaceManager.Instance.Players.Count; i++)
            {
                var player = RaceManager.Instance.Players[i];

                if (player.PlayerType == PlayerType.Player)
                {
                    if (Input.GetKeyDown(player.ControllerScheme.Cancel))
                    {
                        playersToRemove.Add(player);
                    }
                }
            }

            RemovePlayers(playersToRemove);
            playersToRemove.Clear();

            if (InputManager.BackPressed())
            {
                Back();
            }

            if (InputManager.StartPressed())
            {
                StartGame();
            }
        }
    }

    public void AddAI()
    {
        if (RaceManager.Instance.Players.Count < RaceManager.MaxPlayers)
        {
            Player player = new Player();
            player.Name = "AI";
            player.PlayerType = PlayerType.AI;
            player.PrimaryColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            player.SecondaryColor = new Color(Random.Range(0f, 1f), 0, 0);

            var firstEmptySlot = playerSelections.Find(x => x.IsSlotEmpty());
            playerSelections[playerSelections.IndexOf(firstEmptySlot)].Join(player);
            RaceManager.Instance.Players.Add(player);
        }

        UpdateAICount();
    }

    public void RemoveAI()
    {
        var player = RaceManager.Instance.Players.Find(x => x.PlayerType == PlayerType.AI);

        if (player != null)
        {
            RemovePlayers(new List<Player>() { player });
        }

        UpdateAICount();
    }

    private void UpdateAICount()
    {
        AICountLabel.text = RaceManager.Instance.Players.FindAll(m => m.PlayerType == PlayerType.AI).Count.ToString("0");
    }

    public Player CreatePlayer(string name, ControllerScheme scheme)
    {
        Player player = new Player();
        player.Name = name;
        player.ControllerScheme = scheme;
        player.PrimaryColor = Color.red;

        if (playerSelections.Find(x => x.IsControllerSchemeSame(player.ControllerScheme)) == null)
        {
            var firstEmptySlot = playerSelections.Find(x => x.IsSlotEmpty() || x.IsSlotAI());

            if (firstEmptySlot.IsSlotAI())
            {
                RemovePlayers(new List<Player>() { firstEmptySlot.Player });
                UpdateAICount();
            }

            playerSelections[playerSelections.IndexOf(firstEmptySlot)].Join(player);
            RaceManager.Instance.Players.Add(player);
        }

        return player;
    }

    private void RemovePlayers(List<Player> players)
    {
        foreach (var player in players)
        {
            var playerFound = playerSelections.Find(x => x.Player != null && x.Player.ID == player.ID);

            if (playerFound != null)
            {
                playerFound.Leave();
                RaceManager.Instance.Players.Remove(player);
            }
        }
    }
}
