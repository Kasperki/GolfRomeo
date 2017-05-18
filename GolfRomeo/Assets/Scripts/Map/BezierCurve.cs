using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    public Vector2 p0, p1, p2;

    public BezierCurve(Vector2 p0, Vector2 p1, Vector2 p2)
    {
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
    }

    public Vector2 GetPositionAt(float t)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
    }

    public void Draw(float steps)
    {
        for (float i = 0; i < 1.0f; i += steps)
        {
            var start = GetPositionAt(i);
            var end = GetPositionAt(i + steps);
            DrawLines.DrawLine(new Vector3(start.x, 0, start.y), new Vector3(end.x, 0, end.y), Color.yellow);
        }
    }
}
