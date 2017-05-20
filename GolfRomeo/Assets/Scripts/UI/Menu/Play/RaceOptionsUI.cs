using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceOptionsUI : MonoBehaviour
{
    public Text Laps;
    public Text Points;
    public Text FastestLap;
    public Text DamageModifier;
    public Text FuelUsage;
    public Text TiresExhausting;

    public void Init()
    {
        Laps.text = RaceManager.Instance.Laps.ToString();
    }
}
