﻿using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshRenderer))]
public class Checkpoint : MonoBehaviour, IEditable
{
    public int CheckpointOrder;

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

    public MeshRenderer TextRenderer;
    private new MeshRenderer renderer;
    private LapTracker lapTracker { get { return GetComponentInParent<LapTracker>(); } }

    void Awake ()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.enabled = TextRenderer.enabled = GameManager.CheckState(State.Edit);
    }
	
    void OnTriggerEnter(Collider collider)
    {
        var car = collider.gameObject.GetComponentInParent<Car>();
        if (car != null)
        {
            lapTracker.EnterCheckpoint(car, CheckpointOrder);
        }
    }

    public void SetOrder()
    {
        SetOrder(lapTracker.GetComponentsInChildren<Checkpoint>().Length - 1);
    }

    public void SetOrder(int order)
    {
        CheckpointOrder = order;
        TextRenderer.GetComponent<TextMesh>().text = order.ToString();

        if (order == 0)
        {
            renderer.material = (Material)Resources.Load(ResourcesLoader.Roads + "/CheckpointFinishline", typeof(Material)) as Material;
        }
    }

    //IEditable
    public void OnBlur() {}

    public void OnHover() {}

    public void OnSelect(bool selected, Transform target) { }

    public void Move(Transform target, float rotationDelta)
    {
        transform.position = target.position;
        transform.rotation = Quaternion.Euler(target.eulerAngles.x, transform.eulerAngles.y + rotationDelta, target.eulerAngles.z);
    }

    public void OnDelete()
    {
        GetComponentInParent<LapTrackerEditor>().ListenToReOrderOnChange();
        Destroy(gameObject);
    }
}
