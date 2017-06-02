using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorTerrainHeightEditorUI : CursorBaseUI
{
    public CursorUI CursorEditorUI;

    public override void Open()
    {
        buttonsRect.gameObject.SetActive(true);
        CursorEditor.EditMode = EditMode.TerrainHeightMap;
    }

    public override void Close()
    {
        buttonsRect.gameObject.SetActive(false);
        CursorEditorUI.Open();
    }

    public void RaiseTerrain()
    {
        CursorEditor.terrainEditor.TerrainEditMode = TerrainEditType.Raise;
        CursorEditorUI.Exit();
    }

    public void LowerTerrain()
    {
        CursorEditor.terrainEditor.TerrainEditMode = TerrainEditType.Lower;
        CursorEditorUI.Exit();
    }

    public void RaiseTerrainSmooth()
    {
        CursorEditor.terrainEditor.TerrainEditMode = TerrainEditType.RaiseSmooth;
        CursorEditorUI.Exit();
    }

    public void LowerTerrainSmooth()
    {
        CursorEditor.terrainEditor.TerrainEditMode = TerrainEditType.LowerSmooth;
        CursorEditorUI.Exit();
    }

    public void SmoothTerrain()
    {
        CursorEditor.terrainEditor.TerrainEditMode = TerrainEditType.Smooth;
        CursorEditorUI.Exit();
    }
}
