using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointProgressTracker))]
public class WaypointTargetProvider : TargetProvider
{
    private WaypointProgressTracker waypointProgressTracker;

    public void Start()
    {
        waypointProgressTracker = gameObject.GetComponent<WaypointProgressTracker>();
    }

    public override Transform GetTargetTransform()
    {
        return waypointProgressTracker.target;
    }
}
