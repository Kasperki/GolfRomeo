using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrackObject : MonoBehaviour, IEditable
{
    public string ID;

    private bool[] cachedTriggerInfo;
    private bool hover;

    public void OnHover()
    {
        if (!hover)
        {
            var renderer = GetComponent<Renderer>();

            if (renderer)
            {
                var color = renderer.material.GetColor("_Color");
                renderer.material.SetColor("_Color", color - new Color(0.5f, 0.5f, 0.5f, 0));
            }

            foreach (var rendererChildren in gameObject.GetComponentsInChildren<Renderer>())
            {
                var color = rendererChildren.material.GetColor("_Color");
                rendererChildren.material.SetColor("_Color", color - new Color(0.5f, 0.5f, 0.5f, 0));
            }

            hover = true;
        }
    }

    public void OnBlur()
    {
        if (hover)
        {
            var renderer = GetComponent<Renderer>();

            if (renderer)
            {
                var color = renderer.material.GetColor("_Color");
                renderer.material.SetColor("_Color", color + new Color(0.5f, 0.5f, 0.5f, 0));
            }

            foreach (var rendererChildren in gameObject.GetComponentsInChildren<Renderer>())
            {
                var color = rendererChildren.material.GetColor("_Color");
                rendererChildren.material.SetColor("_Color", color + new Color(0.5f, 0.5f, 0.5f, 0));
            }

            hover = false;
        }
    }

    public virtual void OnSelect(bool selected, Transform target)
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

    public void OnDelete()
    {
        Destroy(gameObject);
    }
}
