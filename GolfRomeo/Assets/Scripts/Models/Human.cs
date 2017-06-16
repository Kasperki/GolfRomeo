using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public MeshRenderer BodyMeshRenderer;

    private Vector3 standingPosition;
    private Vector3 awayFromCarDirection;

    private const float Walking_Speed = 50f;
    private const float Running_Speed = 25f;
    private bool running;

    private void Start ()
    {
        standingPosition = transform.position;
        BodyMeshRenderer.material.color = PlayerColors.PrimaryColors[Random.Range(0, PlayerColors.PrimaryColors.Length)];
	}

    private void FixedUpdate()
    {
        if (GameManager.CheckState(State.Game))
        {
            if (running)
            {
                GetComponent<Rigidbody>().AddForce(awayFromCarDirection * Running_Speed);
            }
            else if ((standingPosition - transform.position).magnitude > 0.1f)
            {
                GetComponent<Rigidbody>().AddForce((standingPosition - transform.position).normalized * Walking_Speed);
            }

            var maxVelocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, 0.0001f); //TODO BETTER 
            GetComponent<Rigidbody>().velocity = maxVelocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var car = other.gameObject.GetComponentInParent<Car>();

        if (car)
        {
            running = true;

            int x = car.transform.position.x < transform.position.x ? 1 : -1;
            int z = car.transform.position.z < transform.position.z ? 1 : -1;
            awayFromCarDirection = new Vector3(x, 0, z) * car.CarController.GetComponent<Rigidbody>().velocity.magnitude;               
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Car>())
        {
            running = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        running = false;
    }
}
