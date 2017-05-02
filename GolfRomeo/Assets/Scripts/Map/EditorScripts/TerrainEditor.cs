using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    public Renderer BrushRenderer;

    private const int MIN_BRUSH_SIZE = 1;
    private const int MAX_BRUSH_SIZE = 100;

    public int BrushSize;
    public float TerrainHeightEditModifier = 0.0005f;

    private TerrainHeightEditor terrainHeightEditor;

    private void Awake()
    {
        terrainHeightEditor = GetComponentInChildren<TerrainHeightEditor>();
    }

    public void CCC ()
    {
        BrushSize += (int)Input.mouseScrollDelta.y * 5;
        BrushSize = Mathf.Clamp(BrushSize, MIN_BRUSH_SIZE, MAX_BRUSH_SIZE);
        BrushRenderer.transform.localScale = new Vector3(1 * BrushSize / 10, 1 * BrushSize / 10, 0.5f);
        BrushRenderer.enabled = true;

        //EDIT TERRAIN
        if (Input.GetKeyDown(KeyCode.Y))
        {
            terrainHeightEditor.RaiseTerrain(TerrainHeightEditModifier, BrushSize);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            terrainHeightEditor.RaiseTerrainSmooth(TerrainHeightEditModifier, BrushSize);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            terrainHeightEditor.RaiseTerrainSmooth(-TerrainHeightEditModifier, BrushSize);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            terrainHeightEditor.SmoothTerrain(BrushSize);
        }

        if (Input.GetKey(KeyCode.B))
        {
            terrainHeightEditor.UpdateTerrainTexture(1, BrushSize);
        }
    }
}
