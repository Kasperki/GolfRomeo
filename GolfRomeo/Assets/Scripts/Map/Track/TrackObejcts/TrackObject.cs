﻿using System;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Collider))]
public class TrackObject : MonoBehaviour, IEditable
{
    public string ID;
    public Sprite Icon;
    public bool SoftCollision;

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector3 Rotation
    {
        get { return transform.eulerAngles; }
        set { transform.eulerAngles = value; }
    }

    private bool hover;

    private new Renderer renderer;
    private Renderer[] childRenderers;
    private readonly Color HOVER_COLOR = new Color(2, 2, 2, 1);

    private Color cachedRendererColor;
    private Color[] cachedChildRendererColors;

    private void Awake()
    {
        if (GameManager.CheckState(State.Edit))
        {
            renderer = GetComponent<Renderer>();

            if (renderer)
            {
                cachedRendererColor = renderer.material.GetColor("_Color");
            }

            childRenderers = gameObject.GetComponentsInChildren<Renderer>();
            cachedChildRendererColors = new Color[childRenderers.Length];
            for (int i = 0; i < childRenderers.Length; i++)
            {
                cachedChildRendererColors[i] = childRenderers[i].material.GetColor("_Color");
            }

            var rigidBody = GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                rigidBody.isKinematic = true;
            }

            foreach (var rgdb in GetComponentsInChildren<Rigidbody>())
            {
                rgdb.isKinematic = true;
            }
        }
    }

    public void OnHover()
    {
        if (!hover)
        {
            SetRendererColors(HOVER_COLOR);
            hover = true;
        }
    }

    public void OnBlur()
    {
        if (hover)
        {
            SetRendererDefaultColors();
            hover = false;
        }
    }

    private void SetRendererDefaultColors()
    {
        if (renderer)
        {
            renderer.material.SetColor("_Color", cachedRendererColor);
        }

        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].material.SetColor("_Color", cachedChildRendererColors[i]);
        }
    }

    private void SetRendererColors(Color color)
    {
        if (renderer)
        {
            renderer.material.SetColor("_Color", color);
        }

        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].material.SetColor("_Color", color);
        }
    }

    public virtual void OnSelect(bool selected, Transform target)
    {

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
