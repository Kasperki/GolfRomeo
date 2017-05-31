﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class EditorUI : MonoBehaviour
{
    public MenuUI MenuUI;
    public MapEditorMenuUI MapEditorMenuUI;
    public RectTransform ContentParent;
    public GameObject EditorInfo;

    public RectTransform mapButtonsParent;
    public GameObject MapButtonPrefab;
    private DirectoryHelper directoryHelper;

    private Track track;
    public InputField TrackNameInput;

    public GameObject CursorPrefab;

    private void Start()
    {
        directoryHelper = new DirectoryHelper();
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

        foreach (var track in tracks)
        {
            var obj = Instantiate(MapButtonPrefab);
            obj.transform.SetParent(mapButtonsParent, false);
            obj.GetComponent<MapSelectionEditorButton>().SetListener(track);
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
        RaceManager.Instance.EditTrack(); //LOAD TRACK
        var cursor = Instantiate(CursorPrefab) as GameObject;
        cursor.GetComponent<CursorEditor>().ControlScheme = new ControllerScheme().Keyboard();

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

        ContentParent.gameObject.SetActive(false);
        EditorInfo.SetActive(true);
    }

    public void Update ()
    {
        if (InputManager.BackPressed() && GameManager.CheckState(State.Menu))
        {
            Back();
        }
    }
}
