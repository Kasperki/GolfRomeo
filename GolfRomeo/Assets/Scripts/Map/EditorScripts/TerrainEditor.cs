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

    private TerrainEditorTools terrainHeightEditor;
    private CursorEditor cursorEditor;

    private void Awake()
    {
        terrainHeightEditor = GetComponentInChildren<TerrainEditorTools>();
        brushRendererMesh = BrushRenderer.GetComponent<MeshFilter>();
        cursorEditor = GetComponentInParent<CursorEditor>();
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

        if (Input.GetKey(KeyCode.Space) && cursorEditor.CursorUI.IsActive() == false)
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

        if (Input.GetKey(KeyCode.Space) && cursorEditor.CursorUI.IsActive() == false)
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