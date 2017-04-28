using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines
{
    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    public static void DrawLine(Transform tr, Vector3 startPos, Vector3 endPos, Color color)
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.PushMatrix();

        // Set transformation matrix for drawing to
        // match our transform
        //GL.MultMatrix(tr.localToWorldMatrix);

        // Draw lines
        GL.Begin(GL.LINES);

        // Vertex colors change from red to green
        GL.Color(color);
        // One vertex at transform position
        GL.Vertex3(startPos.x, startPos.y, startPos.z);
        // Another vertex at edge of circle
        GL.Vertex3(endPos.x, endPos.y, endPos.z);

        GL.End();
        GL.PopMatrix();
    }
}
