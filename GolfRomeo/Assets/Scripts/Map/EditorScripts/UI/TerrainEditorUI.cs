using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainEditorUI : CursorEditorUI
{
    private CursorUI CursorEditorUI;

    protected void Start()
    {
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

    public void RaiseTerrain()
    {
        CursorEditor.terrainEditor.TerrainEditMode = TerrainEdit.Raise;
        CursorEditorUI.Exit();
    }

    public void LowerTerrain()
    {
        CursorEditor.terrainEditor.TerrainEditMode = TerrainEdit.Lower;
        CursorEditorUI.Exit();
    }

    public void RaiseTerrainSmooth()
    {
        CursorEditor.terrainEditor.TerrainEditMode = TerrainEdit.RaiseSmooth;
        CursorEditorUI.Exit();
    }

    public void LowerTerrainSmooth()
    {
        CursorEditor.terrainEditor.TerrainEditMode = TerrainEdit.LowerSmooth;
        CursorEditorUI.Exit();
    }

    public void SmoothTerrain()
    {
        CursorEditor.terrainEditor.TerrainEditMode = TerrainEdit.Smooth;
        CursorEditorUI.Exit();
    }
}
