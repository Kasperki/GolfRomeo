using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : Singleton<WeatherManager>
{
    public Light SunLight;
    public Weather Weather;

	public void Initialize()
    {
        Weather.Initialize(SunLight);
    }
}
