using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldObject : MonoBehaviour, IEditable
{
    public string ID;
    public Vector3 Position { get { return transform.position; } }
    public Vector3 Rotation { get { return transform.rotation.eulerAngles; } }

    private bool selected;
    private Transform target;

    private bool[] cachedTriggerInfo;

    public void Update()
    {
        if (selected)
        {
            transform.position = target.position;
        }
    }

    public void OnHover()
    {

    }

    public void OnBlur()
    {

    }

    public void OnSelect(Transform target)
    {
        selected = !selected;
        this.target = target;

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
}
