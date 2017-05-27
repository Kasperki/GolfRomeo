using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorUI : MonoBehaviour
{
    public MenuUI MenuUI;
    public MapEditorMenuUI MapEditorMenuUI;
    public RectTransform ContentParent;
    public GameObject EditButton;
    public GameObject EditorInfo;

    public RectTransform mapButtonsParent;
    public GameObject MapButtonPrefab;
    private DirectoryHelper directoryHelper;

    private Track track;
    public InputField TrackNameInput;

    public GameObject CursorPrefab;

    private bool editing;

    private void Start()
    {
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
            obj.GetComponent<MapSelectionEditorButton>().SetListener(name);
        }
    }

    public void Init()
    {
        gameObject.SetActive(true);
        EditorInfo.SetActive(false);
        ContentParent.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(EditButton);
        Start();

        editing = false;
    }

    public void Back()
    {
        MenuUI.Init();
        ContentParent.gameObject.SetActive(false);
    }

    public void StartEdit()
    {
        GameManager.SetState(State.Edit);
        RaceManager.Instance.EditTrack(); //LOAD TRACK
        var cursor = Instantiate(CursorPrefab) as GameObject;
        cursor.GetComponent<CursorEditor>().ControlScheme = new ControllerScheme().Keyboard();

        editing = true;
        ContentParent.gameObject.SetActive(false);
        EditorInfo.SetActive(true);
    }

    public void NewTrack()
    {
        GameManager.SetState(State.Edit);
        Track.Instance.Name = TrackNameInput.text;
        Track.Instance.ID = Guid.NewGuid(); //TODO INIT SOMEHERE ELSE THAN IN FUCKIN UI
        var cursor = Instantiate(CursorPrefab) as GameObject;

        cursor.GetComponent<CursorEditor>().ControlScheme = new ControllerScheme().Keyboard();
        cursor.GetComponentInChildren<TerrainEditorTools>().NewEmptyTerrain();

        editing = true;
        ContentParent.gameObject.SetActive(false);
        EditorInfo.SetActive(true);
    }

    public void Update ()
    {
        if (InputManager.BackPressed() && editing == false)
        {
            Back(); //TODO ONLY WHEN ACTIVE ON THIS MENU....
        }
    }
}
