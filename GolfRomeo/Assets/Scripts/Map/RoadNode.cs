using System;
using BLINDED_AM_ME;
using UnityEngine;

public class RoadNode : MonoBehaviour, IEditable
{
    Trail_Mesh road;

    void Start ()
    {
        road = GetComponentInParent<Trail_Mesh>();
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
