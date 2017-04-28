using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrackObject : MonoBehaviour, IEditable
{
    public string ID;

    private bool[] cachedTriggerInfo;

    public void OnHover()
    {
        var renderer = GetComponent<Renderer>();

        if (renderer)
        {
            renderer.material.SetColor("_Color", new Color(1, renderer.material.color.g, renderer.material.color.b, 0.15f));
        }

        foreach (var rendererChildren in gameObject.GetComponentsInChildren<Renderer>())
        {
            rendererChildren.material.SetColor("_Color", new Color(1, rendererChildren.material.color.g, rendererChildren.material.color.b, 0.15f));
        }
    }

    public void OnBlur()
    {
        var renderer = GetComponent<Renderer>();

        if (renderer)
        {
            renderer.material.SetColor("_Color", new Color(0, renderer.material.color.g, renderer.material.color.b, 1));
        }

        foreach (var rendererChildren in gameObject.GetComponentsInChildren<Renderer>())
        {
            rendererChildren.material.SetColor("_Color", new Color(0, rendererChildren.material.color.g, rendererChildren.material.color.b, 1));
        }
    }

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

        //transform.rotation = Quaternion.Euler(target.eulerAngles.x, transform.eulerAngles.y + rotationDelta, target.eulerAngles.z);
    }

    public void OnDelete()
    {
        Destroy(gameObject);
    }
}
