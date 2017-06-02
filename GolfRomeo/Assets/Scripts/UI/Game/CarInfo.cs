using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarInfo : MonoBehaviour
{
    public Text Name;
    public Image Background;
    public Image SecondaryBackground;

    public Image Health;
    public Image Fuel;
    public Image Tires;

    public Text CurrentLap;
    public Text FastestLapTime;
    public Text CurrentLapTime;
    public Text Speed;

    public CarRaceData LapInfo;

    private RectTransform rectTransform;

    public void Init(CarRaceData lapInfo)
    {
        LapInfo = lapInfo;
        Name.text = lapInfo.car.Player.Name;

        Background.color = lapInfo.car.Player.PrimaryColor;
        SecondaryBackground.color = lapInfo.car.Player.SecondaryColor;

        rectTransform = GetComponent<RectTransform>();
    }

    public void UpdateLapInformation()
    {
        CurrentLap.text = LapInfo.CurrentLap + "/" + RaceManager.Instance.Laps;
        FastestLapTime.text = TimeSpanHelper.GetTimeInMinutesAndSeconds(LapInfo.FastestLapTime);
        CurrentLapTime.text = TimeSpanHelper.GetTimeInMinutesAndSeconds(LapInfo.LastLapTime);
        Speed.text = LapInfo.car.CarController.CurrentSpeed.ToString("0") + " km/h";
    }

    public void Update()
    {
        UpdateLapInformation();

        Health.fillAmount = LapInfo.car.Health / LapInfo.car.MaxHealth;
        Fuel.fillAmount = LapInfo.car.Fuel / LapInfo.car.MaxFuel;
        Tires.fillAmount = LapInfo.car.TiresHealth / LapInfo.car.TiresMaxHealth;

        if (rectTransform.rect.width > 250)
        {
            rectTransform.sizeDelta = new Vector2(250, rectTransform.sizeDelta.y);
        }
    }
}
