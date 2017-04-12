using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshRenderer))]
public class Checkpoint : MonoBehaviour, IEditable
{
    private LapTracker lapTracker
    {
        get
        {
            return GetComponentInParent<LapTracker>();
        }
    }

    public int CheckpointOrder;
    private Collider collider;
    private MeshRenderer renderer;
    private MeshRenderer orderRenderer;

    private bool selected;
    private Transform target;

    void Awake ()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponent<MeshRenderer>();
        orderRenderer = GetComponentInChildren<MeshRenderer>();
    }
	
    void Update()
    {
        renderer.enabled = orderRenderer.enabled = !GameManager.CheckState(State.Game);

        if (selected)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        var car = collider.gameObject.GetComponent<Car>();
        if (car != null)
        {
            lapTracker.EnterCheckpoint(car.Name, CheckpointOrder);
        }
    }

    public void SetOrder()
    {
        CheckpointOrder = lapTracker.GetComponentsInChildren<Checkpoint>().Length - 1;
        orderRenderer.gameObject.GetComponent<TextMesh>().text = CheckpointOrder.ToString();

        if (CheckpointOrder == 0)
        {
            //SET FINISHLINE MATERIAL TODOO
        }
    }

    public void OnBlur()
    {

    }

    public void OnHover()
    {

    }

    public void OnSelect(Transform target)
    {
        selected = !selected;
        this.target = target;
    }
}
