using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    //TODO CAP SPEED - By offroad, gasoline & damage.
        //Jarruille myös kerroin
    //REFACTOR SHIT

    public float TopSpeed = 200;
    public float TopSpeedOffRoad = 50;

    public float MaxMotorTorque; // maximum torque the motor can apply to wheel
    public float MaxBreakTorque; // maximum torque the motor can apply to wheel
    public float MaxSteeringAngle; // maximum steer angle the wheel can have
    public float MaxReverseTorque; // reverse torque
    public float DownForce; // down force

    public List<AxleInfo> AxleInfos; // the information about each individual axle

    private Rigidbody rgbd;

    public const float SPEED_MULTIPLIER = 5.6f;
    public float CurrentSpeed { get { return rgbd.velocity.magnitude * SPEED_MULTIPLIER; } }

    public ParticleController ParticleController;

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
            GetAxleTerrain(axleInfo);

            //Steering
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steerAngle;
                axleInfo.rightWheel.steerAngle = steerAngle;
            }

            //Driving
            if (axleInfo.motor)
            {
                switch (axleInfo.leftWheelTerrain)
                {
                    case WheelTerrain.Sand:
                        ParticleController.EmitSandParticles();
                        axleInfo.leftWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionSand;
                        break;
                    case WheelTerrain.SandRoad:
                        axleInfo.leftWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionSandRoad;
                        break;
                    case WheelTerrain.Asfalt:
                        axleInfo.leftWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionAsfalt;
                        break;
                    case WheelTerrain.Grass:
                        ParticleController.EmitGrassParticles();
                        axleInfo.leftWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionGrass;
                        break;
                    case WheelTerrain.Ice:
                        axleInfo.leftWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionIce;
                        break;
                    default:
                        axleInfo.leftWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionDefault;
                        break;
                }

                switch (axleInfo.rightWheelTerrain)
                {
                    case WheelTerrain.Sand:
                        ParticleController.EmitSandParticles();
                        axleInfo.rightWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionSand;
                        break;
                    case WheelTerrain.SandRoad:
                        axleInfo.rightWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionSandRoad;
                        break;
                    case WheelTerrain.Asfalt:
                        ParticleController.EmitGrassParticles();
                        axleInfo.rightWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionAsfalt;
                        break;
                    case WheelTerrain.Grass:
                        axleInfo.rightWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionGrass;
                        break;
                    case WheelTerrain.Ice:
                        axleInfo.rightWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionIce;
                        break;
                    default:
                        axleInfo.rightWheel.motorTorque = accel * MaxMotorTorque;
                        break;
                }
            }

            //Breaking
            if (CurrentSpeed > 3 && Vector3.Angle(transform.forward, rgbd.velocity) < 50f)
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

            /*if (axleInfo.handbrake && handbrake > 0f)
            {
                var hbTorque = handbrake * MaxBreakTorque;
                axleInfo.leftWheel.brakeTorque = hbTorque;
                axleInfo.rightWheel.brakeTorque = hbTorque;
            }

            if (footbrake == 0 && handbrake == 0)
            {
                axleInfo.leftWheel.brakeTorque = 0f;
                axleInfo.rightWheel.brakeTorque = 0f;
            }*/

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        CapSpeed();
        AddDownForce(AxleInfos[0]);

        ParticleController.CleanEmmiters();
    }

    private void CapSpeed()
    {
        if (CurrentSpeed > TopSpeed)
        {
            rgbd.velocity = (TopSpeed / SPEED_MULTIPLIER) * rgbd.velocity.normalized;
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

    private void GetAxleTerrain(AxleInfo axle)
    {
        axle.leftWheelTerrain = axle.GetWheelTerrain(axle.leftWheel.transform.position);
        axle.rightWheelTerrain = axle.GetWheelTerrain(axle.rightWheel.transform.position);
    }
}

[Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public WheelTerrain leftWheelTerrain;
    public WheelTerrain rightWheelTerrain;

    public bool handbrake;
    public bool motor;
    public bool steering;

    public float TractionDefault = 1;
    public float TractionAsfalt = 1;
    public float TractionSandRoad = 1;
    public float TractionSand = 1;
    public float TractionGrass = 1;
    public float TractionIce = 1;

    private float[] GetTextureMix(Vector3 worldPos)
    {
        var terrainPos = Track.Instance.Terrain.transform.position;
        var terrainData = Track.Instance.Terrain.terrainData;

        int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth );
        int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight );

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1 );

        // extract the 3D array data to a 1D array:
        float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];
     
         for (int i = 0; i < cellMix.Length; i++)
         {
             cellMix[i] = splatmapData[0, 0, i];
         }
     
         return cellMix;
    }
 
    public WheelTerrain GetWheelTerrain(Vector3 wheelPosition)
    {
        float[] mix = GetTextureMix(wheelPosition);

        float maxMix = 0;
        int terrainIndex = 0;
     
        for (int i = 0; i < mix.Length; i++)
        {
            if (mix[i] > maxMix)
            {
                terrainIndex = i;
                maxMix = mix[i];
            }
        }
     
        return (WheelTerrain)terrainIndex;
    }
}

public enum WheelTerrain
{
    Sand = 0,
    SandRoad = 1,
    Asfalt = 2,
    Grass = 3,
    Ice = 4,
}