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
        renderer.enabled = GameManager.CheckState(State.Edit);
    }

    public void SetStart()
    {
        if (renderer == null)
        {
            renderer = GetComponent<Renderer>();
        }

        renderer.material = (Material)Resources.Load(ResourcesLoader.Roads + "/CheckpointFinishline", typeof(Material)) as Material;
    }

    public void OnBlur() { }

    public void OnHover() { }

    public void OnSelect(bool selected, Transform target)
    {
        Track.Instance.WayPointCircuit.CachePositionsAndDistances();
    }

    public void Move(Transform target, float rotationDelta)
    {
        transform.position = target.position;
    }

    public void OnDelete()
    {
        Destroy(gameObject);
    }
}
