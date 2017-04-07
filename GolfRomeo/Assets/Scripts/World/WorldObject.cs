using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldObject : MonoBehaviour
{
    public string ID;
    public Vector3 Position { get { return transform.position; } }
    public Vector3 Rotation { get { return transform.rotation.eulerAngles; } }


    private bool selected;
    private Transform target;

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
    }
}
