using System;
using BLINDED_AM_ME;
using UnityEngine;

public class RoadNode : MonoBehaviour, IEditable
{
    private Trail_Mesh road;
    private Renderer renderer;

    void Start ()
    {
        road = GetComponentInParent<Trail_Mesh>();
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        renderer.enabled = GameManager.CheckState(State.Edit);
    }

    public void OnBlur() {}

    public void OnHover() {}

    public void OnSelect(bool selected, Transform target)
    {
        if (selected == false)
        {
            road.ShapeIt();
        }
    }

    public void Move(Transform target, float rotationDelta)
    {
        transform.position = target.position;
    }

    public void OnDelete()
    {
        DestroyImmediate(gameObject);
        road.ShapeIt();
    }
}
