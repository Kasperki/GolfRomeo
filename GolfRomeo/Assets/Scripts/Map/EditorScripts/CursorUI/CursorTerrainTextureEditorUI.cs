using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorTerrainTextureEditorUI : CursorBaseUI
{
    private CursorUI CursorEditorUI;

    protected void Start()
    {
        CursorEditorUI = GetComponentInParent<CursorUI>();
    }

    public override void Open()
    {
        ButtonsRect.gameObject.SetActive(true);
        CursorEditor.EditMode = EditMode.TerrainTexture;
    }

    public override void Close()
    {
        ButtonsRect.gameObject.SetActive(false);
        CursorEditorUI.Open();
    }

    public void Asflat()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Asfalt0;
        CursorEditorUI.Exit();
    }

    public void WhiteLine()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Asfalt1;
        CursorEditorUI.Exit();
    }

    public void RedLine()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Asfalt2;
        CursorEditorUI.Exit();
    }

    public void Desert()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Sand0;
        CursorEditorUI.Exit();
    }
}
