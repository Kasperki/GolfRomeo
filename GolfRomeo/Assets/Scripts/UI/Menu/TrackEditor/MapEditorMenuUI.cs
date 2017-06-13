using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapEditorMenuUI : MonoBehaviour
{
    public EditorUI EditorUI;
    public Button SaveButton;

    public void Init()
    {
        gameObject.SetActive(true);
        SaveButton.Select();
    }

    public void DiscardAndExit()
    {
        CleanEditor();

        gameObject.SetActive(false);
        EditorUI.Init();
        GameManager.SetState(State.Menu);
    }

    public void SaveAndExit()
    {
        CleanEditor();
        new TrackLoader(Track.Instance).SaveTrack();

        gameObject.SetActive(false);
        EditorUI.Init();
        GameManager.SetState(State.Menu);
    }

    private void CleanEditor()
    {
        var cursors = FindObjectsOfType<CursorEditor>();
        for (int i = 0; i < cursors.Length; i++)
        {
            Destroy(cursors[i].gameObject);
        }

        var editorCurosrInfos = FindObjectsOfType<EditorCursorInfo>();
        for (int i = 0; i < editorCurosrInfos.Length; i++)
        {
            Destroy(editorCurosrInfos[i].gameObject);
        }

        Destroy(FindObjectOfType<LapTrackerEditor>());
    }
}
