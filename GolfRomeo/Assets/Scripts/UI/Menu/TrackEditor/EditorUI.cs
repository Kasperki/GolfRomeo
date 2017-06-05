﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorUI : MonoBehaviour
{
    public MenuUI MenuUI;
    public MapEditorMenuUI MapEditorMenuUI;
    public ConfirmUI ConfirmDeleteUI;
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

        CreateTrackButtons();
    }

    private void CreateTrackButtons()
    {
        var tracks = directoryHelper.GetAllTracks();
        mapButtonsParent.DestroyChildrens();

        for (int i = 0; i < tracks.Length; i++)
        {
            var obj = Instantiate(MapButtonPrefab);
            obj.transform.SetParent(mapButtonsParent, false);
            obj.GetComponent<MapSelectionEditorButton>().SetListener(tracks[i]);

            if (i == 0)
            {
                EventSystem.current.SetSelectedGameObject(obj);
                RaceManager.Instance.TrackNames[0] = tracks[i];
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

    public void PromptDeleteTrack()
    {
        ConfirmDeleteUI.Init("Do you want to delete: " + RaceManager.Instance.TrackNames[0]);
    }

    public void DeleteTrack()
    {
        new TrackFolderHelper().RemoveTrack(RaceManager.Instance.TrackNames[0]);
        CreateTrackButtons();
    }

    public void CopyTrack()
    {
        new TrackFolderHelper().CopyTrack(RaceManager.Instance.TrackNames[0]);
        CreateTrackButtons();
    }
}
