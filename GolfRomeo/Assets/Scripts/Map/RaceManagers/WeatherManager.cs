using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeatherManager : Singleton<WeatherManager>
{
    public Light SunLight;
    public Transform ParticleParent;
    public List<Weather> Weathers;

    private Weather currentWeather;

	public void Initialize(Weather weather)
    {
        currentWeather = weather;
        currentWeather.Initialize(SunLight);

        if (currentWeather.WeatherParticles != null)
        {
            var obj = Instantiate(currentWeather.WeatherParticles);
            obj.transform.SetParent(ParticleParent, false);
        }
    }

    public void Update()
    {
        if (currentWeather != null && currentWeather.DrivingBehaviour != null)
        {
            currentWeather.DrivingBehaviour.UpdateCarBehaviour(Track.Instance.LapTracker.CarOrder.Select(m => m.car).ToArray());
        }
    }
}
