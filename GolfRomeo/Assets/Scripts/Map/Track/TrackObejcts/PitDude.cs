using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitDude : MonoBehaviour
{
    private Pit pit;
    private Vector3 standingPosition;
    private Rigidbody rdbd;

    private const float Movement_Speed = 20;

    private void Start()
    {
        pit = GetComponentInParent<Pit>();
        standingPosition = transform.position;

        rdbd = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (GameManager.CheckState(State.Game))
        {
            if (pit.CarOnPit)
            {
                if ((pit.transform.position - transform.position).magnitude > 0.5f)
                {
                    rdbd.AddForce((pit.transform.position - transform.position).normalized * Movement_Speed);
                }
            }
            else if ((standingPosition - transform.position).magnitude > 0.1f)
            {
                rdbd.AddForce((standingPosition - transform.position).normalized * Movement_Speed);
            }
        }
    }
}
