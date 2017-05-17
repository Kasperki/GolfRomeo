using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct RaceOptions
{
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
                TiresConsuptionRate = 1,
                FuelConsuptionRate = 1,
                DamageFromEnvironmentRate = 1,
            };
        }
    }
}