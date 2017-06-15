using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    private Vector3 standingPosition;
    private Vector3 awayFromCarDirection;

    private const float Walking_Speed = 10;
    private const float Running_Speed = 25;
    private bool running;

    private void Start ()
    {
        standingPosition = transform.position;
	}

    private void Update()
    {
        if (running)
        {
            transform.position += awayFromCarDirection * Running_Speed * Time.deltaTime;
        }
        else
        {
            //transform.position += (standingPosition - transform.position) * Walking_Speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var car = other.gameObject.GetComponent<Car>();

        if (car)
        {
            running = true;
            var carVelocity = car.CarController.GetComponent<Rigidbody>().velocity; //TODO check what part of trigger the car is, and run to the safest way.
            awayFromCarDirection = Quaternion.AngleAxis(-90, Vector3.up) * carVelocity;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Car>())
        {
            running = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        running = false;
    }
}
