using System;
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
    public GameObject FinishedImage;

    public Text CurrentLap;
    public Text FastestLapTime;
    public Text CurrentLapTime;
    public Text Speed;

    public CarRaceData LapInfo;

    private RectTransform rectTransform;
    private float currentTrackRecord;

    public void Init(CarRaceData lapInfo)
    {
        LapInfo = lapInfo;
        Name.text = lapInfo.car.Player.Name;

        Background.color = lapInfo.car.Player.PrimaryColor;
        SecondaryBackground.color = lapInfo.car.Player.SecondaryColor;

        rectTransform = GetComponent<RectTransform>();

        if (rectTransform.rect.width > 250)
        {
            rectTransform.sizeDelta = new Vector2(250, rectTransform.sizeDelta.y);
        }

        currentTrackRecord = new TrackXMLDataEditor(Track.Instance.Metadata.Name).GetTrackRecord();
    }

    public void UpdateLapInformation()
    {
        CurrentLap.text = LapInfo.CurrentLap + "/" + RaceManager.Instance.RaceOptions.Laps;
        FastestLapTime.text = TimeSpan.FromSeconds(LapInfo.FastestLapTime).GetTimeInMinutesAndSeconds();

        if (GameManager.CheckState(State.Game))
        {
            CurrentLapTime.text = TimeSpan.FromSeconds(LapInfo.CurrentLapTime).GetTimeInMinutesAndSeconds();
        }
        else
        {
            CurrentLapTime.text = "00:00:000";
        }

        if (LapInfo.FastestLapTime > 0 && LapInfo.FastestLapTime < currentTrackRecord)
        {
            FastestLapTime.color = Color.red;
        }


        FinishedImage.SetActive(LapInfo.Finished);

        Speed.text = LapInfo.car.CarController.CurrentSpeed.ToString("0");
    }

    public void Update()
    {
        UpdateLapInformation();

        Health.fillAmount = LapInfo.car.Health / LapInfo.car.MaxHealth;
        Fuel.fillAmount = LapInfo.car.Fuel / LapInfo.car.MaxFuel;
        Tires.fillAmount = LapInfo.car.Tires / LapInfo.car.MaxTires;
    }
}
