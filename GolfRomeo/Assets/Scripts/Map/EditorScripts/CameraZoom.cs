using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Quaternion cachedRotation;

    private void Awake()
    {
        cachedRotation = transform.rotation;
    }

    public void Zoom(float zoomDelta, Vector3 pos)
    {
        if (zoomDelta != 0)
        {
            float zoomLevel = Camera.main.fieldOfView + zoomDelta;
            zoomLevel = Mathf.Clamp(zoomLevel, 20, 60);
            Camera.main.fieldOfView = zoomLevel;

            if (zoomLevel < 60)
            {
                var targetRotation = Quaternion.LookRotation(pos - transform.position);
                Camera.main.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
            }
            else
            {
                ResetCamera();
            }
        }
    }

    public void ResetCamera()
    {
        Camera.main.fieldOfView = 60;
        Camera.main.transform.rotation = cachedRotation;
    }
}
