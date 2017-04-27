using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class UserCarController : MonoBehaviour
{
    private CarController carController;

    private void Start()
    {
        carController = GetComponent<CarController>();
    }

    public void FixedUpdate()
    {
        carController.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Vertical"));
    }
}
