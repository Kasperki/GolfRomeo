using System;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(CarController))]
[RequireComponent(typeof(AIWaypointProgressTracker))]
public class AICarControl : MonoBehaviour
{
    AIWaypointProgressTracker waypointProgressTracker;

    public enum BrakeCondition
    {
        NeverBrake,                 // the car simply accelerates at full throttle all the time.
        TargetDirectionDifference,  // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.
        TargetDistance,             // the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
                                    // head for a stationary target and come to rest when it arrives there.
    }

    // This script provides input to the car controller in the same way that the user control script does.
    // As such, it is really 'driving' the car, with no special physics or animation tricks to make the car behave properly.

    // "wandering" is used to give the cars a more human, less robotic feel. They can waver slightly
    // in speed and direction while driving towards their target.

    [SerializeField]
    [Range(0, 1)]
    private float cautiousSpeedFactor = 0.05f;               // percentage of max speed to use when being maximally cautious
    [SerializeField]
    [Range(0, 180)]
    private float cautiousMaxAngle = 50f;                  // angle of approaching corner to treat as warranting maximum caution
    [SerializeField]
    private float cautiousMaxDistance = 1f;                              // distance at which distance-based cautiousness begins
    [SerializeField]
    private float cautiousAngularVelocityFactor = 30f;                     // how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)
    [SerializeField]
    private float steerSensitivity = 0.05f;                                // how sensitively the AI uses steering input to turn to the desired direction
    [SerializeField]
    private float accelSensitivity = 0.5f;                                // How sensitively the AI uses the accelerator to reach the current desired speed
    [SerializeField]
    private float brakeSensitivity = 1f;                                   // How sensitively the AI uses the brake to reach the current desired speed
    [SerializeField]
    private float lateralWanderDistance = 2f;                              // how far the car will wander laterally towards its target
    [SerializeField]
    private float lateralWanderSpeed = 0.1f;                               // how fast the lateral wandering will fluctuate
    [SerializeField]
    [Range(0, 1)]
    private float accelWanderAmount = 0.15f;                              // how much the cars acceleration will wander
    [SerializeField]
    private float accelWanderSpeed = 0.01f;                                 // how fast the cars acceleration wandering will fluctuate
    [SerializeField]
    private BrakeCondition brakeCondition = BrakeCondition.TargetDistance; // what should the AI consider when accelerating/braking?
    [SerializeField]
    private bool driving;                                                  // whether the AI is currently actively driving or stopped.
    [SerializeField]
    private Transform target;                                              // 'target' the target object to aim for.
    [SerializeField]
    private bool stopWhenTargetReached;                                    // should we stop driving when we reach the target?
    [SerializeField]
    private float reachTargetThreshold = 1;                                // proximity to target to consider we 'reached' it, and stop driving.

    private float randomPerlin;             // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)
    private CarController carController;    // Reference to actual car controller we are controlling
    private float avoidOtherCarTime;        // time until which to avoid the car we recently collided with
    private float avoidOtherCarSlowdown;    // how much to slow down due to colliding with another car, whilst avoiding
    private float avoidPathOffset;          // direction (-1 or 1) in which to offset path to avoid other car, whilst avoiding
    private new Rigidbody rigidbody;
    private Vector3 localTarget;

    private void Awake()
    {
        waypointProgressTracker = GetComponent<AIWaypointProgressTracker>();

        // get the car controller reference
        carController = GetComponent<CarController>();

        // give the random perlin a random value
        randomPerlin = Random.value * 100;

        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        driving = GameManager.CheckState(State.Game);
        target = waypointProgressTracker.target;

        if (target == null || !driving)
        {
            // Car should not be moving,
            carController.Move(0, 0, 0, 1);
        }
        else
        {
            Vector3 fwd = transform.forward;
            if (rigidbody.velocity.magnitude > carController.TopSpeed * 0.1f)
            {
                fwd = rigidbody.velocity;
            }

            float desiredSpeed = carController.TopSpeed;

            // now it's time to decide if we should be slowing down...
            switch (brakeCondition)
            {
                case BrakeCondition.TargetDirectionDifference:
                    {
                        // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.

                        // check out the angle of our target compared to the current direction of the car
                        float approachingCornerAngle = Vector3.Angle(target.forward, fwd);

                        // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
                        float spinningAngle = rigidbody.angularVelocity.magnitude * cautiousAngularVelocityFactor;

                        // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
                        float cautiousnessRequired = Mathf.InverseLerp(0, cautiousMaxAngle,
                                                                        Mathf.Max(spinningAngle,
                                                                                    approachingCornerAngle));
                        desiredSpeed = Mathf.Lerp(carController.TopSpeed, carController.TopSpeed * cautiousSpeedFactor,
                                                    cautiousnessRequired);
                        break;
                    }

                case BrakeCondition.TargetDistance:
                    {
                        // the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
                        // head for a stationary target and come to rest when it arrives there.

                        // check out the distance to target
                        Vector3 delta = target.position - transform.position;
                        float distanceCautiousFactor = Mathf.InverseLerp(cautiousMaxDistance, 0, delta.magnitude);

                        // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
                        float spinningAngle = rigidbody.angularVelocity.magnitude * cautiousAngularVelocityFactor;

                        // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
                        float cautiousnessRequired = Mathf.Max(
                            Mathf.InverseLerp(0, cautiousMaxAngle, spinningAngle), distanceCautiousFactor);
                        desiredSpeed = Mathf.Lerp(carController.TopSpeed, carController.TopSpeed * cautiousSpeedFactor,
                                                    cautiousnessRequired);
                        break;
                    }

                case BrakeCondition.NeverBrake:
                    break;
            }

            // our target position starts off as the 'real' target position
            Vector3 offsetTargetPos = target.position;

            // if are we currently taking evasive action to prevent being stuck against another car:
            if (Time.time < avoidOtherCarTime)
            {
                // slow down if necessary (if we were behind the other car when collision occured)
                desiredSpeed *= avoidOtherCarSlowdown;

                // and veer towards the side of our path-to-target that is away from the other car
                offsetTargetPos += target.right * avoidPathOffset;
            }
            else
            {
                // no need for evasive action, we can just wander across the path-to-target in a random way,
                // which can help prevent AI from seeming too uniform and robotic in their driving
                offsetTargetPos += target.right *
                                    (Mathf.PerlinNoise(Time.time * lateralWanderSpeed, randomPerlin) * 2 - 1) *
                                    lateralWanderDistance;
            }

            //We are stuck we need to back away
            if (previousTarget > Time.time)
            {
                desiredSpeed = -carController.TopSpeed;
            }

            // accel
            float accelBrakeSensitivity = (desiredSpeed < carController.CurrentSpeed) ? brakeSensitivity : accelSensitivity;
            float accel = Mathf.Clamp((desiredSpeed - carController.CurrentSpeed) * accelBrakeSensitivity, -1, 1);
            accel *= (1 - accelWanderAmount) + (Mathf.PerlinNoise(Time.time * accelWanderSpeed, randomPerlin) * accelWanderAmount); //wander

            // steering
            localTarget = transform.InverseTransformPoint(offsetTargetPos);
            float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
            float steer = Mathf.Clamp(targetAngle * steerSensitivity, -1, 1) * Mathf.Sign(carController.CurrentSpeed);

            // feed input to the car controller.
            carController.Move(steer, accel, accel, 0);
        }

        // if appropriate, stop driving when we're close enough to the target.
        if (stopWhenTargetReached)
        {
            driving = (transform.position - target.position).magnitude > reachTargetThreshold;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // detect collision against other cars, so that we can take evasive action
        if (collision.rigidbody != null)
        {
            var otherAI = collision.rigidbody.GetComponent<AICarControl>();
            if (otherAI != null)
            {
                // we'll take evasive action for 1 second
                avoidOtherCarTime = Time.time + 1;

                // but who's in front?...
                if (Vector3.Angle(transform.forward, otherAI.transform.position - transform.position) < 90)
                {
                    // the other ai is in front, so it is only good manners that we ought to brake...
                    avoidOtherCarSlowdown = 0.5f;
                }
                else
                {
                    // we're in front! ain't slowing down for anybody...
                    avoidOtherCarSlowdown = 1;
                }

                // both cars should take evasive action by driving along an offset from the path centre,
                // away from the other car
                var otherCarLocalDelta = transform.InverseTransformPoint(otherAI.transform.position);
                float otherCarAngle = Mathf.Atan2(otherCarLocalDelta.x, otherCarLocalDelta.z);
                avoidPathOffset = lateralWanderDistance * -Mathf.Sign(otherCarAngle);
            }
        }

        //are we stuck on some road object.
        if (collision.gameObject.layer == (int)TrackMask.TrackObjects)
        {
            if (!stuckOnTrackObject)
            {
                stuckOnTrackObject = true;
                stuckOnTrackObjectLast = Time.time + 1;
            } 
        }

        if (Time.time > stuckOnTrackObjectLast && stuckOnTrackObject)
        {
            previousTarget = Time.time + 0.75f;
            waypointProgressTracker.PreviousPoint();
        }
    }

    private bool stuckOnTrackObject;
    private float stuckOnTrackObjectLast, previousTarget;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == (int)TrackMask.TrackObjects)
        {
            stuckOnTrackObject = false;
        }
    }
}
