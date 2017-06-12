using UnityEngine;

public struct RaceOptions
{
    [Range(1,1000)]
    public int Laps;
    [Range(0, 2)]
    public float TiresConsuptionRate;
    [Range(0, 2)]
    public float FuelConsuptionRate;
    [Range(0, 2)]
    public float DamageFromEnvironmentRate;

    public static RaceOptions DefaultRaceOptions
    {
        get
        {
            return new RaceOptions()
            {
                Laps = 3,
                TiresConsuptionRate = 1,
                FuelConsuptionRate = 2.5f,
                DamageFromEnvironmentRate = 1,
            };
        }
    }
}