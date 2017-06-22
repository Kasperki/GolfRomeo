using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainyDriving : WeatherDrivingBehaviour
{
    public override void InitializeCarBehaviour(Car[] cars)
    {
        foreach (var car in cars)
        {
            foreach (var axle in car.CarController.AxleInfos)
            {
                foreach (var wheel in axle.Wheels)
                {
                    wheel.Initialize(0.75f, 0.6f);
                }
            }
        }
    }

    public override void UpdateCarBehaviour(Car[] cars)
    {
        //throw new NotImplementedException();
    }
}
