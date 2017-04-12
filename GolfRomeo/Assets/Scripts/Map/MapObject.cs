using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MapObject : MonoBehaviour, IEditable
{
    public string ID;

    private bool[] cachedTriggerInfo;

    public void OnHover() { }

    public void OnBlur() { }

    public void OnSelect(bool selected, Transform target)
    {
        if (selected)
        {
            var colliders = GetComponents<Collider>();
            cachedTriggerInfo = new bool[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
            {
                cachedTriggerInfo[i] = colliders[i].isTrigger;
                colliders[i].isTrigger = true;
            }
        }
        else
        {
            var colliders = GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].isTrigger = cachedTriggerInfo[i];
            }
        }
    }

    public void Move(Transform target, float rotationDelta)
    {
        transform.position = target.position;
        transform.eulerAngles += new Vector3(0, rotationDelta, 0);
    }
}
