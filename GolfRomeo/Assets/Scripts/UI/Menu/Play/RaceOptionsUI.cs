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
        UpdateLapsCount();
    }

    public void AddLaps()
    {
        RaceManager.Instance.Laps++;
        UpdateLapsCount();
    }

    public void DecreaseLaps()
    {
        if (RaceManager.Instance.Laps > 1)
        {
            RaceManager.Instance.Laps--;
            UpdateLapsCount();
        }
    }

    private void UpdateLapsCount()
    {
        Laps.text = RaceManager.Instance.Laps.ToString();
    }
}
