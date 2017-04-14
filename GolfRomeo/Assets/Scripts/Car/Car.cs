using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Car : MonoBehaviour
{
    public string Name { get { return gameObject.name; } }

    public float Fuel;
    public float FuelUsage;
    public float MaxFuel;

    public float Health;
    public float MaxHealth;


}