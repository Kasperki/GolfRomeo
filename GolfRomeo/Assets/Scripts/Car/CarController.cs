using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(Car))]
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public Car Car;
    public const float SPEED_MULTIPLIER = 5.6f;

    public float TopSpeed = 200;
    public float TopSpeedOffRoad = 50;
    private const float NoFuelMaxSpeed = 10;
    private const float FuelBaseConsuption = 0.1f;

    public float MaxMotorTorque; // maximum torque the motor can apply to wheel
    public float MaxBreakTorque; // maximum torque the motor can apply to wheel
    public float MaxSteeringAngle; // maximum steer angle the wheel can have
    public float MaxReverseTorque; // reverse torque
    public float DownForce; // down force

    public List<AxleInfo> AxleInfos; // the information about each individual axle

    public float CurrentSpeed { get { return rgbd == null ? 0 : rgbd.velocity.magnitude * SPEED_MULTIPLIER; } }

    public CarParticleController ParticleController;

    private Rigidbody rgbd;

    private void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        Car = GetComponent<Car>();

        foreach (AxleInfo axleInfo in AxleInfos)
        {
            axleInfo.Initialize();
        }
    }
    public void Move(float steering, float accel, float footbrake, float handbrake)
    {
        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        accel = Mathf.Clamp(accel, 0, 1);
        footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);

        var steerAngle = steering * MaxSteeringAngle;
        foreach (AxleInfo axleInfo in AxleInfos)
        {
            GetAxleTerrain(axleInfo);

            //Steering
            if (axleInfo.steering)
            {
                float steerRandom = 0;
                if (Car.Health / Car.MaxHealth < 0.5f)
                {
                    float destructionFactor = 0.5f - Car.Health / Car.MaxHealth;
                    steerRandom = UnityEngine.Random.Range(-3, 3) * destructionFactor;
                }

                axleInfo.leftWheel.steerAngle = steerAngle + steerRandom;
                axleInfo.rightWheel.steerAngle = steerAngle + steerRandom;
            }

            //Driving
            if (axleInfo.motor)
            {
                Car.Fuel -= FuelBaseConsuption * Time.deltaTime * RaceManager.Instance.RaceOptions.FuelConsuptionRate * accel;

                switch (axleInfo.leftWheelTerrain)
                {
                    case WheelTerrain.Sand:
                        ParticleController.EmitSandParticles();
                        axleInfo.leftWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionSand;
                        break;
                    case WheelTerrain.SandRoad:
                        ParticleController.EmitSandRoadParticles();
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
                        ParticleController.EmitSandRoadParticles();
                        axleInfo.rightWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionSandRoad;
                        break;
                    case WheelTerrain.Asfalt:
                        axleInfo.rightWheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionAsfalt;
                        break;
                    case WheelTerrain.Grass:
                        ParticleController.EmitGrassParticles();
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

            if (CurrentSpeed > 10)
            {
                float driftValue = Vector3.Dot(rgbd.velocity, transform.forward);   

                if (driftValue < 5f)
                {
                    var force = SkidMarkForcePerSpeed();
                    Car.TiresHealth -= force * RaceManager.Instance.RaceOptions.TiresConsuptionRate * Time.deltaTime;
                    Track.Instance.SkidMarks.AddSkidMarks(axleInfo.leftWheel.transform.position, force);
                    Track.Instance.SkidMarks.AddSkidMarks(axleInfo.rightWheel.transform.position, force);
                }
            }

            //Breaking
            if (CurrentSpeed > 3 && Vector3.Angle(transform.forward, rgbd.velocity) < 50f)
            {
                if (footbrake > 0)
                {
                    var force = SkidMarkForcePerSpeed();
                    Car.TiresHealth -= force * RaceManager.Instance.RaceOptions.TiresConsuptionRate * Time.deltaTime;
                    Track.Instance.SkidMarks.AddSkidMarks(axleInfo.leftWheel.transform.position, force);
                    Track.Instance.SkidMarks.AddSkidMarks(axleInfo.rightWheel.transform.position, force);
                }

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

                    Car.Fuel -= FuelBaseConsuption * Time.deltaTime * RaceManager.Instance.RaceOptions.FuelConsuptionRate * Mathf.Abs(footbrake);
                }
            }

            if (handbrake > 0f)
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

            if (axleInfo.addDownForce == true)
            {
                AddDownForce(axleInfo);
            }

            axleInfo.UpdateExtremumSlip(Car.TiresHealth / Car.TiresMaxHealth);
        }

        CapSpeed();
        ParticleController.CleanEmmiters();
    }

    private float SkidMarkForcePerSpeed()
    {
        return Mathf.Max(0.05f, CurrentSpeed / 100);
    }

    private void CapSpeed()
    {
        float maxSpeed = TopSpeed;

        //OffRoad
        int offRoadTerrain = 0;
        int motorWheelCount = 0;
        foreach (var axle in AxleInfos)
        {
            if (axle.motor)
            {
                motorWheelCount += 2;

                if (axle.leftWheelTerrain != WheelTerrain.Asfalt && axle.leftWheelTerrain != WheelTerrain.SandRoad)
                {
                    offRoadTerrain++;
                }
                if (axle.rightWheelTerrain != WheelTerrain.Asfalt && axle.rightWheelTerrain != WheelTerrain.SandRoad)
                {
                    offRoadTerrain++;
                }
            }
        }

        if (offRoadTerrain == motorWheelCount)
        {
            maxSpeed = TopSpeedOffRoad;
        }

        //Cap from Fuel
        if (Car.Fuel <= 0)
        {
            maxSpeed = NoFuelMaxSpeed;
        }

        //Cap from health
        if (Car.Health < Car.MaxHealth)
        {
            maxSpeed = Mathf.Min(Mathf.Max(5, TopSpeed * (Car.Health / Car.MaxHealth)), maxSpeed);
        }

        //Set rigidbody velocity based on maxspeed
        if (CurrentSpeed > maxSpeed)
        {
            var newVelocity = (maxSpeed / SPEED_MULTIPLIER) * rgbd.velocity.normalized;
            rgbd.velocity = new Vector3(newVelocity.x, rgbd.velocity.y, newVelocity.z);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (CurrentSpeed < 15 && collision.collider.gameObject.layer == (int)TrackMask.TrackObjects)
        {
            var trackObject = collision.collider.gameObject.GetComponent<TrackObject>();

            if (trackObject != null && trackObject.SoftCollision == false)
            {
                Car.Health -= rgbd.velocity.magnitude * 3 * RaceManager.Instance.RaceOptions.DamageFromEnvironmentRate;
            }
        }
    }
}

[Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public WheelTerrain leftWheelTerrain;
    public WheelTerrain rightWheelTerrain;

    public bool motor;
    public bool steering;
    public bool addDownForce;

    [Range(0,2)]
    public float TractionDefault = 1;
    [Range(0, 2)]
    public float TractionAsfalt = 1;
    [Range(0, 2)]
    public float TractionSandRoad = 1;
    [Range(0, 2)]
    public float TractionSand = 1;
    [Range(0, 2)]
    public float TractionGrass = 1;
    [Range(0, 2)]
    public float TractionIce = 1;

    private const float MaxForwardSlip = 2;
    private const float MaxSidewaysSlip = 2;

    public float ExtremumSlipForward
    {
        set
        {
            var leftWheelFrictionCurve =  leftWheel.forwardFriction;
            leftWheelFrictionCurve.extremumSlip = value;
            leftWheel.forwardFriction = leftWheelFrictionCurve;

            var rightWheelFrictionCurve = rightWheel.forwardFriction;
            rightWheelFrictionCurve.extremumSlip = value;
            rightWheel.forwardFriction = rightWheelFrictionCurve;
        }
        get
        {
            return leftWheel.forwardFriction.extremumSlip;
        }
    }

    public float ExtremumSlipSideways
    {
        set
        {
            var leftWheelFrictionCurve = leftWheel.sidewaysFriction;
            leftWheelFrictionCurve.extremumSlip = value;
            leftWheel.sidewaysFriction = leftWheelFrictionCurve;

            var rightWheelFrictionCurve = rightWheel.sidewaysFriction;
            rightWheelFrictionCurve.extremumSlip = value;
            rightWheel.sidewaysFriction = rightWheelFrictionCurve;
        }
        get
        {
            return leftWheel.sidewaysFriction.extremumSlip;
        }
    }

    private float extremumSlipForwardDefault = 0.4f;
    private float extremumSlipSidewaysDefault = 0.2f;

    public void Initialize()
    {
        extremumSlipForwardDefault = leftWheel.forwardFriction.extremumSlip;
        extremumSlipSidewaysDefault = leftWheel.sidewaysFriction.extremumSlip;
    }

    public void UpdateExtremumSlip(float tireHealthPercentage)
    {
        ExtremumSlipForward = Mathf.Lerp(MaxForwardSlip , extremumSlipForwardDefault, tireHealthPercentage);
        ExtremumSlipSideways = Mathf.Lerp(MaxSidewaysSlip, extremumSlipSidewaysDefault, tireHealthPercentage);
    }

    /// <summary>
    /// http://answers.unity3d.com/answers/457390/view.html
    /// </summary>
    /// <param name="worldPos">position</param>
    /// <returns>Array of terrain texture weights</returns>
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

        var terrainTexture = (TerrainTextures)terrainIndex;
        var terrainTextureName = Regex.Replace(terrainTexture.ToString(), @"[\d-]", string.Empty);
        return (WheelTerrain)Enum.Parse(typeof(WheelTerrain), terrainTextureName, true);
    }
}

public enum WheelTerrain
{
    Asfalt = 0,
    SandRoad = 1,
    Sand = 2,
    Grass = 3,
    Snow = 4,
    Ice = 5,
}