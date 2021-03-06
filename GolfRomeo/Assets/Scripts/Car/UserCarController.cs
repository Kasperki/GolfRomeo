﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class UserCarController : MonoBehaviour
{
    private CarController carController;
    private ControllerScheme controllerScheme;

    private void Start()
    {
        carController = GetComponent<CarController>();
        controllerScheme = carController.Car.Player.ControllerScheme;
    }

    public void FixedUpdate()
    {
        if (GameManager.CheckState(State.Game) || GameManager.CheckState(State.Edit))
        {
            carController.Move(Input.GetAxis(controllerScheme.HorizontalAxis), Input.GetAxis(controllerScheme.VerticalAxis), Input.GetAxis(controllerScheme.VerticalAxis), 0);
        }
        else
        {
            carController.Move(0, 0, 0, 1);
        }
    }
}
