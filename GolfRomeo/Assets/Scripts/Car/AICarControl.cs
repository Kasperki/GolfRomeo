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
    private float m_CautiousSpeedFactor = 0.05f;               // percentage of max speed to use when being maximally cautious
    [SerializeField]
    [Range(0, 180)]
    private float m_CautiousMaxAngle = 50f;                  // angle of approaching corner to treat as warranting maximum caution
    [SerializeField]
    private float m_CautiousMaxDistance = 1f;                              // distance at which distance-based cautiousness begins
    [SerializeField]
    private float m_CautiousAngularVelocityFactor = 30f;                     // how cautious the AI should be when considering its own current angular velocity (i.e. easing off acceleration if spinning!)
    [SerializeField]
    private float m_SteerSensitivity = 0.05f;                                // how sensitively the AI uses steering input to turn to the desired direction
    [SerializeField]
    private float m_AccelSensitivity = 0.5f;                                // How sensitively the AI uses the accelerator to reach the current desired speed
    [SerializeField]
    private float m_BrakeSensitivity = 1f;                                   // How sensitively the AI uses the brake to reach the current desired speed
    [SerializeField]
    private float m_LateralWanderDistance = 2f;                              // how far the car will wander laterally towards its target
    [SerializeField]
    private float m_LateralWanderSpeed = 0.1f;                               // how fast the lateral wandering will fluctuate
    [SerializeField]
    [Range(0, 1)]
    private float m_AccelWanderAmount = 0.15f;                  // how much the cars acceleration will wander
    [SerializeField]
    private float m_AccelWanderSpeed = 0.01f;                                 // how fast the cars acceleration wandering will fluctuate
    [SerializeField]
    private BrakeCondition m_BrakeCondition = BrakeCondition.TargetDistance; // what should the AI consider when accelerating/braking?
    [SerializeField]
    private bool m_Driving;                                                  // whether the AI is currently actively driving or stopped.
    [SerializeField]
    private Transform m_Target;                                              // 'target' the target object to aim for.
    [SerializeField]
    private bool m_StopWhenTargetReached;                                    // should we stop driving when we reach the target?
    [SerializeField]
    private float m_ReachTargetThreshold = 1;                                // proximity to target to consider we 'reached' it, and stop driving.

    private float m_RandomPerlin;             // A random value for the car to base its wander on (so that AI cars don't all wander in the same pattern)
    private CarController m_CarController;    // Reference to actual car controller we are controlling
    private float m_AvoidOtherCarTime;        // time until which to avoid the car we recently collided with
    private float m_AvoidOtherCarSlowdown;    // how much to slow down due to colliding with another car, whilst avoiding
    private float m_AvoidPathOffset;          // direction (-1 or 1) in which to offset path to avoid other car, whilst avoiding
    private Rigidbody m_Rigidbody;
    private Vector3 localTarget;

    private void Awake()
    {
        waypointProgressTracker = GetComponent<AIWaypointProgressTracker>();

        // get the car controller reference
        m_CarController = GetComponent<CarController>();

        // give the random perlin a random value
        m_RandomPerlin = Random.value * 100;

        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        m_Driving = GameManager.CheckState(State.Game);
        m_Target = waypointProgressTracker.target;

        if (m_Target == null || !m_Driving)
        {
            // Car should not be moving,
            m_CarController.Move(0, 0, 0, 1);
        }
        else
        {
            Vector3 fwd = transform.forward;
            if (m_Rigidbody.velocity.magnitude > m_CarController.TopSpeed * 0.1f)
            {
                fwd = m_Rigidbody.velocity;
            }

            float desiredSpeed = m_CarController.TopSpeed;

            // now it's time to decide if we should be slowing down...
            switch (m_BrakeCondition)
            {
                case BrakeCondition.TargetDirectionDifference:
                    {
                        // the car will brake according to the upcoming change in direction of the target. Useful for route-based AI, slowing for corners.

                        // check out the angle of our target compared to the current direction of the car
                        float approachingCornerAngle = Vector3.Angle(m_Target.forward, fwd);

                        // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
                        float spinningAngle = m_Rigidbody.angularVelocity.magnitude * m_CautiousAngularVelocityFactor;

                        // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
                        float cautiousnessRequired = Mathf.InverseLerp(0, m_CautiousMaxAngle,
                                                                        Mathf.Max(spinningAngle,
                                                                                    approachingCornerAngle));
                        desiredSpeed = Mathf.Lerp(m_CarController.TopSpeed, m_CarController.TopSpeed * m_CautiousSpeedFactor,
                                                    cautiousnessRequired);
                        break;
                    }

                case BrakeCondition.TargetDistance:
                    {
                        // the car will brake as it approaches its target, regardless of the target's direction. Useful if you want the car to
                        // head for a stationary target and come to rest when it arrives there.

                        // check out the distance to target
                        Vector3 delta = m_Target.position - transform.position;
                        float distanceCautiousFactor = Mathf.InverseLerp(m_CautiousMaxDistance, 0, delta.magnitude);

                        // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
                        float spinningAngle = m_Rigidbody.angularVelocity.magnitude * m_CautiousAngularVelocityFactor;

                        // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
                        float cautiousnessRequired = Mathf.Max(
                            Mathf.InverseLerp(0, m_CautiousMaxAngle, spinningAngle), distanceCautiousFactor);
                        desiredSpeed = Mathf.Lerp(m_CarController.TopSpeed, m_CarController.TopSpeed * m_CautiousSpeedFactor,
                                                    cautiousnessRequired);
                        break;
                    }

                case BrakeCondition.NeverBrake:
                    break;
            }

            // our target position starts off as the 'real' target position
            Vector3 offsetTargetPos = m_Target.position;

            // if are we currently taking evasive action to prevent being stuck against another car:
            if (Time.time < m_AvoidOtherCarTime)
            {
                // slow down if necessary (if we were behind the other car when collision occured)
                desiredSpeed *= m_AvoidOtherCarSlowdown;

                // and veer towards the side of our path-to-target that is away from the other car
                offsetTargetPos += m_Target.right * m_AvoidPathOffset;
            }
            else
            {
                // no need for evasive action, we can just wander across the path-to-target in a random way,
                // which can help prevent AI from seeming too uniform and robotic in their driving
                offsetTargetPos += m_Target.right *
                                    (Mathf.PerlinNoise(Time.time * m_LateralWanderSpeed, m_RandomPerlin) * 2 - 1) *
                                    m_LateralWanderDistance;
            }

            //We are stuck we need to back away
            if (previousTarget > Time.time)
            {
                desiredSpeed = -m_CarController.TopSpeed;
            }

            // accel
            float accelBrakeSensitivity = (desiredSpeed < m_CarController.CurrentSpeed) ? m_BrakeSensitivity : m_AccelSensitivity;
            float accel = Mathf.Clamp((desiredSpeed - m_CarController.CurrentSpeed) * accelBrakeSensitivity, -1, 1);
            accel *= (1 - m_AccelWanderAmount) + (Mathf.PerlinNoise(Time.time * m_AccelWanderSpeed, m_RandomPerlin) * m_AccelWanderAmount); //wander

            // steering
            localTarget = transform.InverseTransformPoint(offsetTargetPos);
            float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
            float steer = Mathf.Clamp(targetAngle * m_SteerSensitivity, -1, 1) * Mathf.Sign(m_CarController.CurrentSpeed);

            // feed input to the car controller.
            m_CarController.Move(steer, accel, accel, 0);
        }

        // if appropriate, stop driving when we're close enough to the target.
        if (m_StopWhenTargetReached)
        {
            m_Driving = (transform.position - m_Target.position).magnitude > m_ReachTargetThreshold;
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
                m_AvoidOtherCarTime = Time.time + 1;

                // but who's in front?...
                if (Vector3.Angle(transform.forward, otherAI.transform.position - transform.position) < 90)
                {
                    // the other ai is in front, so it is only good manners that we ought to brake...
                    m_AvoidOtherCarSlowdown = 0.5f;
                }
                else
                {
                    // we're in front! ain't slowing down for anybody...
                    m_AvoidOtherCarSlowdown = 1;
                }

                // both cars should take evasive action by driving along an offset from the path centre,
                // away from the other car
                var otherCarLocalDelta = transform.InverseTransformPoint(otherAI.transform.position);
                float otherCarAngle = Mathf.Atan2(otherCarLocalDelta.x, otherCarLocalDelta.z);
                m_AvoidPathOffset = m_LateralWanderDistance * -Mathf.Sign(otherCarAngle);
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
