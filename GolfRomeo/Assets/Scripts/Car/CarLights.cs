using UnityEngine;

public class CarLights : MonoBehaviour
{
    public Light[] FrontLights;
    public Light[] BackLights;
    public Light BreakLight;

    public void Reset()
    {
        BreakLight.enabled = false;
    }

	public void ShowBreakLight()
    {
        BreakLight.enabled = true;
    }
}
