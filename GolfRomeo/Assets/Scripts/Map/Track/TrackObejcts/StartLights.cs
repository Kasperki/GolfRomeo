using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLights : MonoBehaviour
{
    public Light Light;

	void Update ()
    {
		if (GameManager.CheckState(State.Game))
        {
            Light.color = Color.green;
            GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.green);
        }
        else
        {
            Light.color = Color.red;
            GetComponent<Renderer>().materials[2].SetColor("_EmissionColor", Color.red);
        }
	}
}
