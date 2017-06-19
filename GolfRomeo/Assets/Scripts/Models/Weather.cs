using System;
using UnityEngine;

[Serializable]
public class Weather
{
    public string Name;
    public Color SunColor;
    public float SunIntensity;

    public ParticleSystem WeatherParticles;
    public WeatherDrivingBehaviour DrivingBehaviour;

    public virtual void Initialize(Light sunLight)
    {
        sunLight.color = SunColor;
        sunLight.intensity = SunIntensity;
    }
}

public abstract class WeatherDrivingBehaviour : MonoBehaviour
{
    public abstract void UpdateCarBehaviour(Car[] car);
}