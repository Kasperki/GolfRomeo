using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    public TerrainEdit TerrainEditMode;
    public TextureEdit TextureEditMode;
    public int TextureID;

    public Renderer BrushRenderer;
    private MeshFilter brushRendererMesh;

    private const int MIN_BRUSH_SIZE = 1;
    private const int MAX_BRUSH_SIZE = 100;

    public int BrushSize = 25;
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

    private BezierCurve UpdateBezierCurve(Vector3 startPosition, Vector3 tangentPos, Vector3 endPosition, int size)
    {
        startPosition = new Vector2(startPosition.x, startPosition.z);
        endPosition = new Vector2(endPosition.x, endPosition.z);
        tangentPos = new Vector2(tangentPos.x, tangentPos.z);

        var bezierCurve = new BezierCurve(startPosition, tangentPos, endPosition);

        //Draw helper lines
        float steps = 0.02f;
        bezierCurve.Draw(steps);

        return bezierCurve;
    }

    public void UpdateTerrainHeightMap ()
    {
        UpdateBrush();

        if (Input.GetKey(cursorEditor.ControlScheme.Select) && cursorEditor.CursorUI.IsActive() == false)
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

    private Vector3 p0, p1, p2;
    private int bezierStatus = 0;

    public void StartBezierEditMode()
    {
        TextureEditMode = TextureEdit.Bezier;
        p0 = transform.position;
        bezierStatus = 0;
    }

    public void UpdateTerrainTexture()
    {
        UpdateBrush();

        switch (TextureEditMode)
        {
            case TextureEdit.Brush:
                if (Input.GetKey(cursorEditor.ControlScheme.Select) && cursorEditor.CursorUI.IsActive() == false)
                {
                    terrainHeightEditor.UpdateTerrainTexture(TextureID, BrushSize);
                }
                break;
            case TextureEdit.Bezier:

                var bezierCurve = UpdateBezierCurve(p0, p1, p2, BrushSize);

                if (Input.GetKeyDown(cursorEditor.ControlScheme.Select) && cursorEditor.CursorUI.IsActive() == false)
                {
                    if (bezierStatus == 0)
                    {
                        p2 = transform.position;
                        p1 = new Vector2(p2.x / 2, 2);
                    }
                    else if (bezierStatus == 1)
                    {
                        p1 = transform.position;
                    }
                    else
                    {
                        terrainHeightEditor.UpdateTerrainTextureOnBezierCurvePath(TextureID, bezierCurve, BrushSize);
                    }

                    bezierStatus++;
                }

                break;
            default:
                break;
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

public enum TextureEdit
{
    Brush = 0,
    Bezier = 1,
}