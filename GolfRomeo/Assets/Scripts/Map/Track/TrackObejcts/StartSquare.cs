using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSquare : TrackObject
{
    private new Renderer renderer;

    void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
        renderer.enabled = GameManager.CheckState(State.Edit);
    }

    public override void OnSelect(bool selected, Transform target)
    {
        base.OnSelect(selected, target);

        if (selected == false)
        {
            transform.position += new Vector3(0, 0.1f, 0);
        }
    }
}
