using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorZoomUI : CursorBaseUI
{
    private CursorUI CursorEditorUI;

    protected new void Start()
    {
        base.Start();
        CursorEditorUI = GetComponentInParent<CursorUI>();
    }

    public override void Open()
    {
        ButtonsRect.gameObject.SetActive(true);
        CursorEditor.EditMode = EditMode.TerrainHeightMap;
    }

    public override void Close()
    {
        ButtonsRect.gameObject.SetActive(false);
        CursorEditorUI.Open();
    }

    public void Zoom1X()
    {
        FindObjectOfType<CameraZoom>().Zoom1X(CursorEditor.transform);
        CursorEditorUI.Exit();
    }

    public void Zoom2X()
    {
        FindObjectOfType<CameraZoom>().Zoom2X(CursorEditor.transform);
        CursorEditorUI.Exit();
    }

    public void Zoom4X()
    {
        FindObjectOfType<CameraZoom>().Zoom4X(CursorEditor.transform);
        CursorEditorUI.Exit();
    }
}
