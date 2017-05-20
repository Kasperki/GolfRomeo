using System.Collections;
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
            obj.GetComponent<MapSelectionButton>().SetListener(name);
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
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                CreatePlayer("Joystick1", new ControllerScheme().Keyboard());
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                CreatePlayer("Joystick2", new ControllerScheme().Keyboard2());
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

            if (Input.GetKeyDown(KeyCode.P))
            {
                Back();
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

            var firstEmptySlot = playerSelections.Find(x => x.IsSlotEmpty());
            playerSelections[playerSelections.IndexOf(firstEmptySlot)].Join(player);
            RaceManager.Instance.Players.Add(player);
        }
    }

    public void RemoveAI()
    {
        var player = RaceManager.Instance.Players.Find(x => x.PlayerType == PlayerType.AI);

        if (player != null)
        {
            RemovePlayers(new List<Player>() { player });
        }
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
