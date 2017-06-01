using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorTerrainTextureEditorUI : CursorBaseUI
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
        CursorEditor.EditMode = EditMode.TerrainTexture;
    }

    public override void Close()
    {
        ButtonsRect.gameObject.SetActive(false);
        CursorEditorUI.Open();
    }

    private void ExitCursorEditorUI()
    {
        ButtonsRect.gameObject.SetActive(false);
        CursorEditorUI.Exit();
    }

    public void Asflat()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Asfalt0;
        ExitCursorEditorUI();
    }

    public void WhiteLine()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Asfalt1;
        ExitCursorEditorUI();
    }

    public void RedLine()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Asfalt2;
        ExitCursorEditorUI();
    }

    public void YellowLine()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Asfalt3;
        ExitCursorEditorUI();
    }

    public void BlackLine()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Asfalt4;
        ExitCursorEditorUI();
    }

    public void Desert()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Sand0;
        ExitCursorEditorUI();
    }

    public void Sand()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Sand1;
        ExitCursorEditorUI();
    }

    public void Grass0()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Grass0;
        ExitCursorEditorUI();
    }

    public void Grass1()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.Grass1;
        ExitCursorEditorUI();
    }

    public void SandRoad0()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.SandRoad0;
        ExitCursorEditorUI();
    }

    public void SandRoad1()
    {
        CursorEditor.terrainEditor.TextureID = (int)TerrainTextures.SandRoad1;
        ExitCursorEditorUI();
    }
}
