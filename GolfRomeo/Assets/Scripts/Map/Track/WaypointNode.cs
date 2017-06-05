using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WaypointNode : MonoBehaviour, IEditable
{
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }


    private new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        renderer.enabled = GameManager.CheckState(State.Edit); //TODO SET TO START / AWAKE? We should not need to call this on race, just when instantiated on edit mode.
    }

    public void SetStart()
    {
        if (renderer == null)
        {
            renderer = GetComponent<Renderer>();
        }

        renderer.material = (Material)Resources.Load(ResourcesLoader.ROADS + "/CheckpointFinishline", typeof(Material)) as Material;
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
