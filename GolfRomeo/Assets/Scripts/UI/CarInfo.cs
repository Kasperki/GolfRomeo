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

    private LapInfo lapInfo;

    public void Init(LapInfo lapInfo)
    {
        this.lapInfo = lapInfo;
        Name.text = lapInfo.car.Player.Name;

        Background.color = lapInfo.car.Player.PrimaryColor;
        SecondaryBackground.color = lapInfo.car.Player.SecondaryColor;
    }

    public void UpdateLapInformation()
    {

    }
}
