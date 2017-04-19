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
        Name.text = lapInfo.car.Name;

        Background.color = lapInfo.car.PrimaryColor;
        SecondaryBackground.color = lapInfo.car.SecondaryColor;
    }

    public void UpdateLapInformation()
    {

    }
}
