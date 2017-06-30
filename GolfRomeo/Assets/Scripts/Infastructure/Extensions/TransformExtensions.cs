using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class TransformExtensions
{
    public static void DestroyChildren(this Transform transform)
    {
        foreach (var childTransform in transform.GetComponentsInChildren<Transform>())
        {
            if (childTransform.transform.GetInstanceID() != transform.GetInstanceID())
            {
                UnityEngine.GameObject.Destroy(childTransform.gameObject);
            }
        }
    }
}
