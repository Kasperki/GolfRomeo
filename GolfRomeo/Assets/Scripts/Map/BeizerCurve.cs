using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeizerCurve
{
    public Vector2 p0, p1, p2;

    public Vector2 GetPositionAt(float t)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
    }
}
