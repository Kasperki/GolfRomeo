using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
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

        BrushSize += (int)Input.mouseScrollDelta.y * 5;
        BrushSize = Mathf.Clamp(BrushSize, MIN_BRUSH_SIZE, MAX_BRUSH_SIZE);

        BrushRenderer.transform.position = new Vector3(transform.position.x, -5.75f, transform.position.z);
        BrushRenderer.transform.localScale = new Vector3(1 * BrushSize / 10, 1 * BrushSize / 10, 1);

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

    public void UpdateTerrainEditorTools ()
    {
        UpdateBrush();

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
