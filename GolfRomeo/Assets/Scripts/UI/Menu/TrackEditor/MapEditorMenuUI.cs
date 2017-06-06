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
        CleanEditorCursors();
        gameObject.SetActive(false);
        EditorUI.Init();
        GameManager.SetState(State.Menu);
    }

    public void SaveAndExit()
    {
        CleanEditorCursors();
        new TrackLoader(Track.Instance).SaveTrack();
        gameObject.SetActive(false);
        EditorUI.Init();
        GameManager.SetState(State.Menu);
    }

    private void CleanEditorCursors()
    {
        var cursors = FindObjectsOfType<CursorEditor>();
        for (int i = 0; i < cursors.Length; i++)
        {
            Destroy(cursors[i].gameObject);
        }
    }
}
