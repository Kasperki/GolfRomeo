using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObjectsUI : CursorBaseUI
{
    private CursorUI CursorEditorUI;
    public CursorTrackObjectsUI TrackObjectsUI;

    protected new void Start()
    {
        base.Start();
        CursorEditorUI = GetComponentInParent<CursorUI>();
    }

    public override void Open()
    {
        ButtonsRect.gameObject.SetActive(true);
        CursorEditor.EditMode = EditMode.Objects;
    }

    public override void Close()
    {
        ButtonsRect.gameObject.SetActive(false);
        CursorEditorUI.Open();
    }

    public void EditObjects()
    {
        TrackObjectsUI.Open();
        ButtonsRect.gameObject.SetActive(false);
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