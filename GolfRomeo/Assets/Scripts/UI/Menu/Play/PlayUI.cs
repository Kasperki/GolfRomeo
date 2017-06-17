using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*
    TODO
    - HOW TO JOIN TO GAME INFO SHOULD BE BETTER, HOW MANY CONTROLLERS ARE CONNECTED ETC.
*/

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

    private TrackFolderHelper directoryHelper;

    private void Awake()
    {
        directoryHelper = new TrackFolderHelper();
    }

    public void Init()
    {
        GameManager.SetState(State.Menu);

        gameObject.SetActive(true);
        ContentParent.gameObject.SetActive(true);
        ContentParent.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        ContentParent.GetComponent<RectTransform>().offsetMax = Vector2.zero;

        RaceManager.Instance.InitializeSettings();
        CreateTrackSelectionButtons(directoryHelper.GetAllTracks());
        RaceOptionsUI.Init();
    }

    private void CreateTrackSelectionButtons(string[] tracks)
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
            StartCoroutine(MoveCurtain(new Vector3(0, 3000, 0)));
        }
    }

    IEnumerator MoveCurtain(Vector3 position)
    {
        while (ContentParent.transform.position.y < position.y)
        {
            ContentParent.transform.position += Vector3.up * 6;
            yield return null;
        }

        ContentParent.gameObject.SetActive(false);
        yield return null;
    }

    void Update()
    {
        if (GameManager.CheckState(State.Menu))
        {
            int setNameSlot = 0;

            for (int i = InputManager.Instance.GetAvailableControllerSchemes().Count; i < playerSelections.Count; i++)
            {
                playerSelections[i].JoinInfo.text = "ADD NEW CONTROLLER";
            }

            foreach (var controllerScheme in InputManager.Instance.GetAvailableControllerSchemes()) 
            {
                int playerSlot = GetFreePlayerSlot(controllerScheme, setNameSlot);

                if (playerSlot != -1)
                {
                    playerSelections[playerSlot].JoinInfo.text = "PRESS " + controllerScheme.Submit.ToString() + " TO JOIN";
                    setNameSlot++;
                }

                if (Input.GetKeyDown(controllerScheme.Submit))
                {
                    AddPlayer(controllerScheme.Name, controllerScheme); //TODO SET PLAYER NAME
                }
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

    private int GetFreePlayerSlot(ControllerScheme controllerScheme, int next)
    {
        if (playerSelections.Find(x => x.IsControllerSchemeSame(controllerScheme)) == null)
        {
            var players = playerSelections.FindAll(x => x.IsSlotEmpty());
            return playerSelections.IndexOf(players[next]);
        }

        return -1;
    }

    public void AddAI()
    {
        if (RaceManager.Instance.Players.Count < RaceManager.MaxPlayers)
        {
            Player player = new Player();
            player.Name = "AI";
            player.PlayerType = PlayerType.AI;

            var index = playerSelections.FindIndex(x => x.IsSlotEmpty());
            AddPlayer(player, index);
        }
    }

    public void RemoveAI()
    {
        var player = RaceManager.Instance.Players.FindAll(x => x.PlayerType == PlayerType.AI).Last();

        if (player != null)
        {
            RemovePlayer(player);
        }
    }

    public void AddPlayer(string name, ControllerScheme scheme)
    {
        Player player = new Player();
        player.Name = name;
        player.ControllerScheme = scheme;

        if (playerSelections.Find(x => x.IsControllerSchemeSame(player.ControllerScheme)) == null)
        {
            int index = playerSelections.FindIndex(x => x.IsSlotEmpty());

            if (index == -1)
            {
                index = playerSelections.FindIndex(x => x.IsSlotAI());
                if (index != -1)
                {
                    RemovePlayer(playerSelections[index].Player);
                }
            }

            if (index != -1)
            {
                AddPlayer(player, index);
            }
        }
    }

    public void AddPlayer(Player player, int index)
    {
        player.PrimaryColor = PlayerColors.PrimaryColors[index];

        playerSelections[index].Join(player);
        RaceManager.Instance.Players.Add(player);

        RaceOptionsUI.UpdateAICount();
    }

    public void RemovePlayer(Player player)
    {
        var playerFound = playerSelections.Find(x => x.Player != null && x.Player.ID == player.ID);

        if (playerFound != null)
        {
            playerFound.Leave();
            RaceManager.Instance.Players.Remove(player);
        }

        RaceOptionsUI.UpdateAICount();
    }
}
