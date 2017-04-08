using BLINDED_AM_ME;
using UnityEngine;

public class RoadNode : MonoBehaviour, IEditable
{
    Trail_Mesh road;
    private bool selected;
    private Transform target;

    void Start ()
    {
        road = GetComponentInParent<Trail_Mesh>();
	}
	
	void Update ()
    {
		if (selected)
        {
            transform.position = target.position;
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

        if (selected == false)
        {
            road.ShapeIt();
        }
    }
}
