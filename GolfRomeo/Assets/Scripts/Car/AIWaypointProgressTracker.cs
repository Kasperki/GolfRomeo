﻿using System;
using UnityEngine;


public class AIWaypointProgressTracker : MonoBehaviour
{
    // This script can be used with any object that is supposed to follow a
    // route marked out by waypoints.

    // This script manages the amount to look ahead along the route,
    // and keeps track of progress and laps.

    [SerializeField]
    private WayPointCircuit circuit; // A reference to the waypoint-based route we should follow

    [SerializeField]
    private float lookAheadForTargetOffset = 2;
    // The offset ahead along the route that the we will aim for

    [SerializeField]
    private float lookAheadForTargetFactor = .2f;
    // A multiplier adding distance ahead along the route to aim for, based on current speed

    [SerializeField]
    private float lookAheadForSpeedOffset = 4;
    // The offset ahead only the route for speed adjustments (applied as the rotation of the waypoint target transform)

    [SerializeField]
    private float lookAheadForSpeedFactor = .2f;
    // A multiplier adding distance ahead along the route for speed adjustments

    // these are public, readable by other objects - i.e. for an AI to know where to head!
    public WayPointCircuit.RoutePoint progressPoint { get; private set; }

    public Transform target;

    private float progressDistance; // The progress round the route, used in smooth mode.
    private Vector3 lastPosition; // Used to calculate current speed (since we may not have a rigidbody component)
    private float speed; // current speed of this object (calculated from delta since last frame)

    // setup script properties
    private void Start()
    {
        // we use a transform to represent the point to aim for, and the point which
        // is considered for upcoming changes-of-speed. This allows this component
        // to communicate this information to the AI without requiring further dependencies.

        // You can manually create a transform and assign it to this component *and* the AI,
        // then this component will update it, and the AI can read it.
        if (target == null)
        {
            target = new GameObject(name + " Waypoint Target").transform;
        }

        circuit = FindObjectOfType<WayPointCircuit>();
        Reset();
    }

    // reset the object to sensible values
    public void Reset()
    {
        progressDistance = 0;
    }

    public void PreviousPoint()
    {
        progressDistance--;
    }

    private void Update()
    {
        // determine the position we should currently be aiming for
        // (this is different to the current progress position, it is a a certain amount ahead along the route)
        // we use lerp as a simple way of smoothing out the speed over time.
        if (Time.deltaTime > 0)
        {
            speed = Mathf.Lerp(speed, (lastPosition - transform.position).magnitude / Time.deltaTime, Time.deltaTime);
        }

        target.position = circuit.GetRoutePoint(progressDistance + lookAheadForTargetOffset + lookAheadForTargetFactor * speed).position;
        target.rotation = Quaternion.LookRotation(circuit.GetRoutePoint(progressDistance + lookAheadForSpeedOffset + lookAheadForSpeedFactor * speed).direction);

        // get our current progress along the route
        progressPoint = circuit.GetRoutePoint(progressDistance);
        Vector3 progressDelta = progressPoint.position - transform.position;
        if (Vector3.Dot(progressDelta, progressPoint.direction) < 0)
        {
            progressDistance += progressDelta.magnitude * 0.5f;
        }

        lastPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(target.position, target.position + target.forward);
        }
    }
}
