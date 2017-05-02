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

    public void CCC ()
    {
        //SET BRUSH GRAPHICS
        BrushSize += (int)Input.mouseScrollDelta.y * 5;
        BrushSize = Mathf.Clamp(BrushSize, MIN_BRUSH_SIZE, MAX_BRUSH_SIZE);

        BrushRenderer.transform.eulerAngles = new Vector3(-90,0,0);
        BrushRenderer.transform.position = new Vector3(BrushRenderer.transform.position.x, -4.5f, BrushRenderer.transform.position.z);
        BrushRenderer.transform.localScale = new Vector3(1 * BrushSize / 10, 1 * BrushSize / 10, 1);
        BrushRenderer.enabled = true;

        Vector3[] vertices = brushRendererMesh.mesh.vertices;
        var verticesCopy = new Vector3[vertices.Length];

        for (int i = 0; i < verticesCopy.Length; i++)
        {
            verticesCopy[i] = BrushRenderer.transform.TransformPoint(vertices[i]);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var d = terrainHeightEditor.GetTerrainHeightAtPos(transform.position);
            Debug.Log("mousePos" + transform.position + "height:" + d);

            for (int i = 0; i < 20; i++)
            {
                var k = terrainHeightEditor.GetTerrainHeightAtPos(verticesCopy[i]);
                Debug.Log("verticesPosition" + verticesCopy[i] + "height:" + k);
            }
        }

        verticesCopy = terrainHeightEditor.CoordinatesToTerrain(verticesCopy);

        for (int i = 0; i < verticesCopy.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x, vertices[i].y, verticesCopy[i].z);
        }

        BrushRenderer.GetComponent<MeshFilter>().mesh.vertices = vertices;


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
