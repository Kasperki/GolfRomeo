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

    public RectTransform mapButtonsParent;
    public GameObject MapButtonPrefab;
    private DirectoryHelper directoryHelper;

    private Track track;
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
            obj.GetComponent<MapSelectionEditorButton>().SetListener(name);
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
        Instantiate(CursorPrefab);

        ContentParent.gameObject.SetActive(false);
    }

    public void NewTrack()
    {
        Track.Instance.ID = Guid.NewGuid(); //TODO INIT NET TRACK TOOD GET NAME
        Instantiate(CursorPrefab);

        ContentParent.gameObject.SetActive(false);
    }

    public void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back(); //TODO ONLY WHEN ACTIVE ON THIS MENU....
        }
    }
}
