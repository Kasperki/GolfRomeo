using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarInfo : MonoBehaviour
{
    public Text Name;
    public Image Background;
    public Image SecondaryBackground;

    public Text CurrentLap;
    public Text FastestLapTime;
    public Text CurrentLapTime;
    public Text Speed;

    public LapInfo LapInfo;

    public void Init(LapInfo lapInfo)
    {
        LapInfo = lapInfo;
        Name.text = lapInfo.car.Player.Name;

        Background.color = lapInfo.car.Player.PrimaryColor;
        SecondaryBackground.color = lapInfo.car.Player.SecondaryColor;
    }

    public void UpdateLapInformation()
    {
        CurrentLap.text = LapInfo.CurrentLap.ToString();
        FastestLapTime.text = LapInfo.FastestLapTime.ToString(Constants.LAP_TIME_FORMAT);
        CurrentLapTime.text = LapInfo.LastLapTime.ToString(Constants.LAP_TIME_FORMAT);
        Speed.text = LapInfo.car.CarController.CurrentSpeed.ToString("0") + " km/h";
    }

    public void Update()
    {
        UpdateLapInformation();
    }
}
