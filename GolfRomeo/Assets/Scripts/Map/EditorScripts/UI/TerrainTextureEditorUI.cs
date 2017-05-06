using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainTextureEditorUI : EditorUI
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


    public void Desert()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Desert;
        CursorEditorUI.Exit();
    }

    public void Asflat()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Asflat;
        CursorEditorUI.Exit();
    }

    public void WhiteLine()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.WhiteLine;
        CursorEditorUI.Exit();
    }
}
