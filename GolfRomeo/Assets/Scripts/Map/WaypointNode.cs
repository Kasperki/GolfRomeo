using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WaypointNode : MonoBehaviour, IEditable
{
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        renderer.enabled = GameManager.CheckState(State.Edit);
    }

    public void OnBlur() { }

    public void OnHover() { }

    public void OnSelect(bool selected, Transform target) { }

    public void Move(Transform target, float rotationDelta)
    {
        transform.position = target.position;
    }

    public void OnDelete()
    {
        Destroy(gameObject);
    }
}
