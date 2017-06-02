using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBrushUI : CursorBaseUI
{
    public CursorUI CursorEditorUI;
    public CursorTerrainTextureEditorUI TerrainTextureEditor;

    public override void Open()
    {
        buttonsRect.gameObject.SetActive(true);
    }

    public override void Close()
    {
        buttonsRect.gameObject.SetActive(false);
        CursorEditorUI.Open();
    }

    public void NormalBrush()
    {
        CursorEditor.terrainEditor.StartBrushEditMode();
        TerrainTextureEditor.Open();
        buttonsRect.gameObject.SetActive(false);
    }

    public void BezierBrush()
    {
        CursorEditor.terrainEditor.StartBezierEditMode();
        TerrainTextureEditor.Open();
        buttonsRect.gameObject.SetActive(false);
    }
}
