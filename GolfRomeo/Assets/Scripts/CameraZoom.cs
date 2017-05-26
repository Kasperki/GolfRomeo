using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Quaternion cachedRotation;
    private Transform target;

    private bool zooming;

    private void Awake()
    {
        cachedRotation = transform.rotation;
    }

	public void Zoom1X(Transform target)
    {
        transform.rotation = cachedRotation;
        Camera.main.fieldOfView = 60;
        zooming = false;
    } 

    public void Zoom2X(Transform target)
    {
        this.target = target;
        Camera.main.fieldOfView = 40;
        zooming = true;
    }

    public void Zoom4X(Transform target)
    {
        this.target = target;
        Camera.main.fieldOfView = 20;
        zooming = true;
    }

    public void Update()
    {
        if (zooming)
        {
            transform.LookAt(target);
        }
    }
}
