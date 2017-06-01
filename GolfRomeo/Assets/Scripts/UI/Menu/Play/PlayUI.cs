using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    public MenuUI MenuUI;
    public RaceOptionsUI RaceOptionsUI;
    public RectTransform ContentParent;

    //Player selection
    public List<PlayerSelectionUI> playerSelections;

    //Track selection
    public RectTransform mapButtonsParent;
    public GameObject MapButtonPrefab;
    public Button[] ButtonsToMapSelectionNavigation;

    private DirectoryHelper directoryHelper;

    private void Start()
    {
        directoryHelper = new DirectoryHelper();
    }

    public void Init()
    {
        GameManager.SetState(State.Menu);

        gameObject.SetActive(true);
        ContentParent.gameObject.SetActive(true);
        ContentParent.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        ContentParent.GetComponent<RectTransform>().offsetMax = Vector2.zero;

        RaceManager.Instance.InitializeSettings();
        CreateMapButtons(directoryHelper.GetAllTracks());
        RaceOptionsUI.Init();
    }

    private void CreateMapButtons(string[] tracks)
    {
        mapButtonsParent.DestroyChildrens();

        for (int i = 0; i < tracks.Length; i++)
        {
            var obj = Instantiate(MapButtonPrefab);
            obj.transform.SetParent(mapButtonsParent, false);
            obj.GetComponent<MapSelectionButton>().SetListener(tracks[i]);

            if (i == 0)
            {
                foreach (var button in ButtonsToMapSelectionNavigation)
                {
                    var navigation = button.navigation;
                    navigation.selectOnDown = obj.GetComponent<Button>();
                    button.navigation = navigation;
                }
            }
        }
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

       //UpdateAICount();
    }


    public void RemoveAI()
    {
        var player = RaceManager.Instance.Players.Find(x => x.PlayerType == PlayerType.AI);

        if (player != null)
        {
            RemovePlayer(player);
        }

        //UpdateAICount();
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
                RemovePlayer(firstEmptySlot.Player);
                //UpdateAICount();
            }

            playerSelections[playerSelections.IndexOf(firstEmptySlot)].Join(player);
            RaceManager.Instance.Players.Add(player);
        }

        return player;
    }

    public void RemovePlayer(Player player)
    {
        var playerFound = playerSelections.Find(x => x.Player != null && x.Player.ID == player.ID);

        if (playerFound != null)
        {
            playerFound.Leave();
            RaceManager.Instance.Players.Remove(player);
        }
    }
}
