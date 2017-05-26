using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    Quaternion cachedRotation;

    private void Awake()
    {
        cachedRotation = transform.rotation;
    }

	public void Zoom1X(Transform target)
    {
        transform.rotation = cachedRotation;
        Camera.main.fieldOfView = 60;
    } 

    public void Zoom2X(Transform target)
    {
        transform.LookAt(target);
        Camera.main.fieldOfView = 40;
    }

    public void Zoom4X(Transform target)
    {
        transform.LookAt(target);
        Camera.main.fieldOfView = 20;
    }
}
