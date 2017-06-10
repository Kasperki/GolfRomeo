using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    public TerrainEditType TerrainEditMode;
    public TextureEditType TextureEditMode;
    public int TextureID;

    public Renderer BrushRenderer;
    private MeshFilter brushRendererMesh;

    private const int Min_Brush_Size = 1;
    private const int Max_Brush_Size = 100;

    public int BrushSize = 25;
    public float TerrainHeightEditModifier = 0.0005f;

    private TerrainEditorTools terrainHeightEditor;
    private CursorEditor cursorEditor;

    private Vector3 p0, p1, p2;
    private int bezierStatus = 0;

    private void Awake()
    {
        terrainHeightEditor = GetComponentInChildren<TerrainEditorTools>();
        brushRendererMesh = BrushRenderer.GetComponent<MeshFilter>();
        cursorEditor = GetComponentInParent<CursorEditor>();
    }

    private void UpdateBrush()
    {
        BrushRenderer.enabled = true;

        if (cursorEditor.CursorUI.IsActive())
        {
            BrushSize += (int)(Input.mouseScrollDelta.y * 1.5f);
            BrushSize = Mathf.Clamp(BrushSize, Min_Brush_Size, Max_Brush_Size);
        }

        BrushRenderer.transform.position = new Vector3(transform.position.x, -5.85f, transform.position.z);
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

        brushRendererMesh.mesh.vertices = vertices;
    }

    private BezierCurve UpdateBezierCurve(Vector3 startPosition, Vector3 tangentPos, Vector3 endPosition, int size)
    {
        startPosition = new Vector2(startPosition.x, startPosition.z);
        endPosition = new Vector2(endPosition.x, endPosition.z);
        tangentPos = new Vector2(tangentPos.x, tangentPos.z);

        return new BezierCurve(startPosition, tangentPos, endPosition);
    }

    public void UpdateTerrainHeightMap ()
    {
        UpdateBrush();

        if (Input.GetKey(cursorEditor.ControlScheme.Submit) && cursorEditor.CursorUI.IsActive() == false)
        {
            switch (TerrainEditMode)
            {
                case TerrainEditType.Raise:
                    terrainHeightEditor.RaiseTerrain(TerrainHeightEditModifier, BrushSize);
                    break;
                case TerrainEditType.Lower:
                    terrainHeightEditor.RaiseTerrain(-TerrainHeightEditModifier, BrushSize);
                    break;
                case TerrainEditType.RaiseSmooth:
                    terrainHeightEditor.RaiseTerrainSmooth(TerrainHeightEditModifier, BrushSize);
                    break;
                case TerrainEditType.LowerSmooth:
                    terrainHeightEditor.RaiseTerrainSmooth(-TerrainHeightEditModifier, BrushSize);
                    break;
                case TerrainEditType.Smooth:
                    terrainHeightEditor.SmoothTerrain(BrushSize);
                    break;
                default:
                    break;
            }
        }
    }

    public void StartBrushEditMode()
    {
        TextureEditMode = TextureEditType.Brush;
    }

    public void StartBezierEditMode()
    {
        TextureEditMode = TextureEditType.Bezier;
        p0 = transform.position;
        p2 = p1 = p0;
        bezierStatus = -1;
    }

    public void OnRenderObject()
    {
        if (TextureEditMode == TextureEditType.Bezier && cursorEditor.CursorUI.IsActive() == false && bezierStatus > -1)
        {
            var bezierCurve = UpdateBezierCurve(p0, p1, p2, BrushSize);

            //Draw helper lines
            float steps = 0.02f;
            bezierCurve.Draw(steps, p0.y);
        }
    }

    public void UpdateTerrainTexture()
    {
        UpdateBrush();

        switch (TextureEditMode)
        {
            case TextureEditType.Brush:
                if (Input.GetKey(cursorEditor.ControlScheme.Submit) && cursorEditor.CursorUI.IsActive() == false)
                {
                    terrainHeightEditor.UpdateTerrainTexture(TextureID, BrushSize);
                }
                break;
            case TextureEditType.Bezier:

                var bezierCurve = UpdateBezierCurve(p0, p1, p2, BrushSize);

                if (bezierStatus == 0)
                {
                    p0 = transform.position;
                    p2 = p1 = p0;
                }
                else if (bezierStatus == 1)
                {
                    p2 = transform.position;
                    p1 = p2;
                }
                else if (bezierStatus == 2)
                {
                    p1 = transform.position;
                }

                //Next bezier curve part
                if (Input.GetKeyDown(cursorEditor.ControlScheme.Submit) && cursorEditor.CursorUI.IsActive() == false)
                {
                    if (bezierStatus == 2)
                    {
                        terrainHeightEditor.UpdateTerrainTextureOnBezierCurvePath(TextureID, bezierCurve, BrushSize);
                    }
                    else if (bezierStatus == 3)
                    {
                        StartBezierEditMode();
                    }

                    bezierStatus++;
                }

                //Reset bezier curve back
                if (Input.GetKeyDown(cursorEditor.ControlScheme.Cancel) && cursorEditor.CursorUI.IsActive() == false)
                {
                    if (bezierStatus >= 0)
                    {
                        bezierStatus--;
                    }

                    if (bezierStatus == -1)
                    {
                        StartBezierEditMode();
                    }
                }
                break;
            default:
                break;
        }
    }
}