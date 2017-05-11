using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorUI : MonoBehaviour
{
    public MenuUI MenuUI;
    public RectTransform ContentParent;
    public GameObject EditButton;

    public RectTransform mapButtonsParent;
    public GameObject MapButtonPrefab;
    private DirectoryHelper directoryHelper;

    public Text TrackName;
    public Text TrackInfo;

    public GameObject CursorPrefab;

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
            obj.GetComponent<MapEditorButton>().SetListener(name);
        }
    }

    public void Init()
    {
        gameObject.SetActive(true);
        ContentParent.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(EditButton);
        Start();
    }

    public void Back()
    {
        MenuUI.Init();
        ContentParent.gameObject.SetActive(false);
    }

    public void StartEdit()
    {
        RaceManager.Instance.EditTrack(); //LOAD TRACK
        var obj = Instantiate(CursorPrefab) as GameObject;

        gameObject.SetActive(false);
    }

    public void NewTrack()
    {
        Track.Instance.ID = Guid.NewGuid(); //TODO INIT NET TRACK TOOD GET NAME
        var obj = Instantiate(CursorPrefab) as GameObject;

        gameObject.SetActive(false);
    }

    public void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
}
