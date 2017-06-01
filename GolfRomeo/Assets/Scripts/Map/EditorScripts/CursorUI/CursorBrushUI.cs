using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBrushUI : CursorBaseUI
{
    private CursorUI CursorEditorUI;
    public CursorTerrainTextureEditorUI TerrainTextureEditor;

    protected new void Start()
    {
        base.Start();
        CursorEditorUI = GetComponentInParent<CursorUI>();
    }

    public override void Open()
    {
        ButtonsRect.gameObject.SetActive(true);
    }

    public override void Close()
    {
        ButtonsRect.gameObject.SetActive(false);
        CursorEditorUI.Open();
    }

    public void NormalBrush()
    {
        CursorEditor.terrainEditor.StartBrushEditMode();
        TerrainTextureEditor.Open();
        ButtonsRect.gameObject.SetActive(false);
    }

    public void BezierBrush()
    {
        CursorEditor.terrainEditor.StartBezierEditMode();
        TerrainTextureEditor.Open();
        ButtonsRect.gameObject.SetActive(false);
    }
}
