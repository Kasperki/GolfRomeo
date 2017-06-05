using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorUI : MonoBehaviour
{
    public MenuUI MenuUI;
    public MapEditorMenuUI MapEditorMenuUI;
    public RectTransform ContentParent;
    public GameObject EditorInfo;

    public RectTransform mapButtonsParent;
    public GameObject MapButtonPrefab;
    private TrackFolderHelper directoryHelper;

    private Track track;
    public InputField TrackNameInput;

    public GameObject CursorPrefab;

    private void Awake()
    {
        directoryHelper = new TrackFolderHelper();
    }

    public void Init()
    {
        gameObject.SetActive(true);
        EditorInfo.SetActive(false);
        ContentParent.gameObject.SetActive(true);

        CreateTrackButtons(directoryHelper.GetAllTracks());
    }

    private void CreateTrackButtons(string[] tracks)
    {
        mapButtonsParent.DestroyChildrens();

        for (int i = 0; i < tracks.Length; i++)
        {
            var obj = Instantiate(MapButtonPrefab);
            obj.transform.SetParent(mapButtonsParent, false);
            obj.GetComponent<MapSelectionEditorButton>().SetListener(tracks[i]);

            if (i == 0)
            {
                EventSystem.current.SetSelectedGameObject(obj);
            }
        }
    }

    public void Back()
    {
        MenuUI.Init();
        ContentParent.gameObject.SetActive(false);
    }

    public void StartEdit()
    {
        GameManager.SetState(State.Edit);
        RaceManager.Instance.EditTrack();

        CreateCursorEditor();

        ContentParent.gameObject.SetActive(false);
        EditorInfo.SetActive(true);
    }

    public void NewTrack()
    {
        GameManager.SetState(State.Edit);
        Track.Instance.NewTrack(TrackNameInput.text);

        CreateCursorEditor();

        ContentParent.gameObject.SetActive(false);
        EditorInfo.SetActive(true);
    }

    private CursorEditor CreateCursorEditor()
    {
        var cursor = Instantiate(CursorPrefab) as GameObject;
        var cursorEditor = cursor.GetComponent<CursorEditor>();
        cursorEditor.ControlScheme = new ControllerScheme().Keyboard();

        return cursorEditor;
    }

    public void Update ()
    {
        if (InputManager.BackPressed() && GameManager.CheckState(State.Menu))
        {
            Back();
        }
    }
}
