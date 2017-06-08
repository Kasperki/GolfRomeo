using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceOptionsUI : MonoBehaviour
{
    public Text Laps;
    public Text AICount;
    public Text DamageModifier;
    public Text FuelUsage;
    public Text TiresExhausting;

    public void Init()
    {
        UpdateLapsCount();
    }

    public void AddLaps()
    {
        RaceManager.Instance.RaceOptions.Laps++;
        UpdateLapsCount();
    }

    public void DecreaseLaps()
    {
        if (RaceManager.Instance.RaceOptions.Laps > 1)
        {
            RaceManager.Instance.RaceOptions.Laps--;
            UpdateLapsCount();
        }
    }

    private void UpdateLapsCount()
    {
        Laps.text = RaceManager.Instance.RaceOptions.Laps.ToString();
    }

    public void UpdateAICount()
    {
        AICount.text = RaceManager.Instance.Players.FindAll(m => m.PlayerType == PlayerType.AI).Count.ToString("0");
    }

    public void DecreaseDamageModifier()
    {
        RaceManager.Instance.RaceOptions.DamageFromEnvironmentRate -= 0.01f;
        UpdateDamageModifierText();
    }

    public void IncreaseDamageModifier()
    {
        RaceManager.Instance.RaceOptions.DamageFromEnvironmentRate += 0.01f;
        UpdateDamageModifierText();
    }

    private void UpdateDamageModifierText()
    {
        DamageModifier.text = RaceManager.Instance.RaceOptions.DamageFromEnvironmentRate.ToString("0.00");
    }

    public void DecreaseFuelConsuption()
    {
        RaceManager.Instance.RaceOptions.FuelConsuptionRate -= 0.01f;
        UpdateFuelConsuptionText();
    }

    public void IncreaseFuelConsuption()
    {
        RaceManager.Instance.RaceOptions.FuelConsuptionRate += 0.01f;
        UpdateFuelConsuptionText();
    }

    private void UpdateFuelConsuptionText()
    {
        FuelUsage.text = RaceManager.Instance.RaceOptions.FuelConsuptionRate.ToString("0.00");
    }

    public void DecreaseTierConsuptionRate()
    {
        RaceManager.Instance.RaceOptions.TiresConsuptionRate -= 0.01f;
        UpdateTierConsuptionRateText();
    }

    public void IncreaseTierConsuptionRate()
    {
        RaceManager.Instance.RaceOptions.TiresConsuptionRate += 0.01f;
        UpdateTierConsuptionRateText();
    }

    private void UpdateTierConsuptionRateText()
    {
        TiresExhausting.text = RaceManager.Instance.RaceOptions.TiresConsuptionRate.ToString("0.00");
    }
}
