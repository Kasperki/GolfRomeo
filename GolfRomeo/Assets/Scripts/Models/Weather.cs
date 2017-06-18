using System;
using UnityEngine;

[Serializable]
public class Weather
{
    public WeatherType Type;
    public Color SunColor;
    public float SunIntensity;

    public ParticleSystem WeatherParticles;

    public void Initialize(Light sunLight)
    {
        sunLight.color = SunColor;
        sunLight.intensity = SunIntensity;
    }
}