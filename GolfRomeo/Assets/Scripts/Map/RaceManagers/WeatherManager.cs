using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeatherManager : Singleton<WeatherManager>
{
    public Light SunLight;
    public Transform ParticleParent;
    public List<Weather> Weathers;

    private Weather currentWeather;
    private int weatherIndex;

    private Car[] cars;

	public void Initialize(Car[] cars)
    {
        currentWeather = GetWeather();
        currentWeather.Initialize(SunLight);
        ParticleParent.transform.DestroyChildren();
        this.cars = cars;

        if (currentWeather.WeatherParticles != null)
        {
            var obj = Instantiate(currentWeather.WeatherParticles);
            obj.transform.SetParent(ParticleParent, false);
        }

        if (currentWeather.DrivingBehaviour != null)
        {
            currentWeather.DrivingBehaviour.InitializeCarBehaviour(cars);
        }
    }

    public void Update()
    {
        if (GameManager.CheckState(State.Game))
        {
            if (currentWeather != null && currentWeather.DrivingBehaviour != null)
            {
                currentWeather.DrivingBehaviour.UpdateCarBehaviour(cars);
            }
        }
    }

    public void PreviousWeather()
    {
        weatherIndex--;

        if (weatherIndex < 0)
        {
            weatherIndex = Weathers.Count;
        }
    }

    public void NextWeather()
    {
        weatherIndex++;

        if (weatherIndex > Weathers.Count)
        {
            weatherIndex = 0;
        }
    }

    private Weather GetWeather()
    {
        if (weatherIndex == Weathers.Count)
        {
            return Weathers[Random.Range(0, Weathers.Count)];
        }

        return Weathers[weatherIndex];
    }

    public string GetWeatherText()
    {
        if (weatherIndex == Weathers.Count)
        {
            return "Random";
        }

        return Weathers[weatherIndex].Name;
    }
}
