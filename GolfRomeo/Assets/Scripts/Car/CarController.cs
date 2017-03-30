using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float TopSpeed = 200;

    public float MaxMotorTorque; // maximum torque the motor can apply to wheel
    public float MaxBreakTorque; // maximum torque the motor can apply to wheel
    public float MaxSteeringAngle; // maximum steer angle the wheel can have
    public float MaxReverseTorque; // reverse torque
    public float DownForce; // down force

    public List<AxleInfo> AxleInfos; // the information about each individual axle

    private Rigidbody rgbd;

    public float CurrentSpeed { get { return rgbd.velocity.magnitude * 2.23693629f; } }

    private void Start()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    public void Move(float steering, float accel, float footbrake, float handbrake)
    {
        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        accel = Mathf.Clamp(accel, 0, 1);
        footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        handbrake = Mathf.Clamp(handbrake, 0, 1);

        var steerAngle = steering * MaxSteeringAngle;
        foreach (AxleInfo axleInfo in AxleInfos)
        {
            //Steering
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steerAngle;
                axleInfo.rightWheel.steerAngle = steerAngle;
            }

            //Driving
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = accel * MaxMotorTorque;
                axleInfo.rightWheel.motorTorque = accel * MaxMotorTorque;
            }

            //Breaking
            if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, rgbd.velocity) < 50f)
            {
                axleInfo.leftWheel.brakeTorque = MaxBreakTorque * footbrake;
                axleInfo.rightWheel.brakeTorque = MaxBreakTorque * footbrake;
            }
            else if (footbrake > 0)
            {
                axleInfo.leftWheel.brakeTorque = 0f;
                axleInfo.rightWheel.brakeTorque = 0f;

                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = -MaxReverseTorque * footbrake;
                    axleInfo.rightWheel.motorTorque = -MaxReverseTorque * footbrake;
                }
            }

            if (axleInfo.handbrake && handbrake > 0f)
            {
                var hbTorque = handbrake * MaxBreakTorque;
                axleInfo.leftWheel.brakeTorque = hbTorque;
                axleInfo.rightWheel.brakeTorque = hbTorque;
            }

            if (footbrake == 0 && handbrake == 0)
            {
                axleInfo.leftWheel.brakeTorque = 0f;
                axleInfo.rightWheel.brakeTorque = 0f;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        CapSpeed();
        AddDownForce(AxleInfos[0]);
    }

    private void CapSpeed()
    {
        float speed = rgbd.velocity.magnitude * 3.6f;

        if (speed > TopSpeed)
        {
            rgbd.velocity = (TopSpeed / 3.6f) * rgbd.velocity.normalized;
        }
    }

    // this is used to add more grip in relation to speed
    private void AddDownForce(AxleInfo axelInfo)
    {
        axelInfo.leftWheel.attachedRigidbody.AddForce(-transform.up * DownForce * axelInfo.leftWheel.attachedRigidbody.velocity.magnitude);
        axelInfo.rightWheel.attachedRigidbody.AddForce(-transform.up * DownForce * axelInfo.rightWheel.attachedRigidbody.velocity.magnitude);
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.rotation = rotation;
    }
}

[Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool handbrake; //does handbrake affect here?
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}

