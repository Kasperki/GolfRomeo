using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    public TerrainEdit TerrainEditMode;
    public int TextureID;

    public Renderer BrushRenderer;
    private MeshFilter brushRendererMesh;

    private const int MIN_BRUSH_SIZE = 1;
    private const int MAX_BRUSH_SIZE = 100;

    public int BrushSize;
    public float TerrainHeightEditModifier = 0.0005f;

    private TerrainHeightEditor terrainHeightEditor;

    private void Awake()
    {
        terrainHeightEditor = GetComponentInChildren<TerrainHeightEditor>();
        brushRendererMesh = BrushRenderer.GetComponent<MeshFilter>();
    }

    private void UpdateBrush()
    {
        BrushRenderer.enabled = true;

        BrushSize += (int)(Input.mouseScrollDelta.y * 1.5f);
        BrushSize = Mathf.Clamp(BrushSize, MIN_BRUSH_SIZE, MAX_BRUSH_SIZE);

        BrushRenderer.transform.position = new Vector3(transform.position.x, -5.65f, transform.position.z);
        BrushRenderer.transform.localScale = new Vector3(BrushSize / 6.8f, BrushSize / 6.8f, 1);
        BrushRenderer.transform.eulerAngles = new Vector3(-90, 0, 0);

        UpdateBrushCursorMesh();
    }

    private void UpdateBrushCursorMesh()
    {
        Vector3[] vertices = brushRendererMesh.mesh.vertices;
        var verticesCopy = new Vector3[vertices.Length];

        for (int i = 0; i < verticesCopy.Length; i++)
        {
            verticesCopy[i] = BrushRenderer.transform.TransformPoint(vertices[i]);
        }

        verticesCopy = terrainHeightEditor.CoordinatesToTerrain(verticesCopy);

        for (int i = 0; i < verticesCopy.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x, vertices[i].y, verticesCopy[i].z);
        }

        BrushRenderer.GetComponent<MeshFilter>().mesh.vertices = vertices;
    }

    public void UpdateTerrainHeightMap ()
    {
        UpdateBrush();

        if (Input.GetKey(KeyCode.Space))
        {
            switch (TerrainEditMode)
            {
                case TerrainEdit.Raise:
                    terrainHeightEditor.RaiseTerrain(TerrainHeightEditModifier, BrushSize);
                    break;
                case TerrainEdit.Lower:
                    terrainHeightEditor.RaiseTerrain(-TerrainHeightEditModifier, BrushSize);
                    break;
                case TerrainEdit.RaiseSmooth:
                    terrainHeightEditor.RaiseTerrainSmooth(TerrainHeightEditModifier, BrushSize);
                    break;
                case TerrainEdit.LowerSmooth:
                    terrainHeightEditor.RaiseTerrainSmooth(-TerrainHeightEditModifier, BrushSize);
                    break;
                case TerrainEdit.Smooth:
                    terrainHeightEditor.SmoothTerrain(BrushSize);
                    break;
                default:
                    break;
            }
        }
    }

    public void UpdateTerrainTexture()
    {
        UpdateBrush();

        if (Input.GetKey(KeyCode.Space))
        {
            terrainHeightEditor.UpdateTerrainTexture(TextureID, BrushSize);
        }
    }
}

public enum TerrainEdit
{
    Raise = 0,
    Lower = 1,
    RaiseSmooth = 2,
    LowerSmooth = 3,
    Smooth = 4
}

public enum TerrainTextures
{
    Asfalt0 = 0,
    Asfalt1 = 1,
    Asfalt2 = 2,
    Asfalt3 = 3,
    Asfalt4 = 4,
    Asfalt5 = 5,
    SandRoad0 = 6,
    SandRoad1 = 7,
    SandRoad2 = 8,
    Sand0 = 9,
    Sand1 = 10,
    Sand2 = 11,
    Grass0 = 12,
    Grass1 = 13,
    Grass2 = 14,
    Snow0 = 15,
    Snow1 = 16,
    Snow2 = 17,
    Ice0 = 18,
    Ice1 = 19,
    Ice2 = 20,
}