using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObjectsUI : CursorBaseUI
{
    public CursorUI CursorEditorUI;
    public CursorTrackObjectsUI TrackObjectsUI;

    public override void Open()
    {
        buttonsRect.gameObject.SetActive(true);
        CursorEditor.EditMode = EditMode.Objects;
    }

    public override void Close()
    {
        buttonsRect.gameObject.SetActive(false);
        CursorEditorUI.Open();
    }

    public void MoveObjects()
    {
        buttonsRect.gameObject.SetActive(false);
        CursorEditorUI.Exit();
    }

    public void EditObjects()
    {
        TrackObjectsUI.Open();
        buttonsRect.gameObject.SetActive(false);
    }

    public void EditWaypoints()
    {
        CursorEditor.CreateWaypoint();
        CursorEditorUI.Exit();
    }

    public void EditCheckpoints()
    {
        CursorEditor.CreateCheckpoint();
        CursorEditorUI.Exit();
    }
}