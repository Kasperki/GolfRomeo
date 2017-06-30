using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(Car))]
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public Car Car;
    public CarLights Lights;
    public const float Speed_Multipler = 5.6f;

    public float TopSpeed = 200;
    public float TopSpeedOffRoad = 50;
    private const float NoFuelMaxSpeed = 10;
    private const float FuelBaseConsuption = 0.01f;

    private const float CrashObjectsDamageMultiplier = 3.0f;
    private const float CrashSlowerCarDamageMultiplier = 2.55f;
    private const float CrashFasterCarDamageMultiplier = 1.25f;

    public float MaxMotorTorque; // maximum torque the motor can apply to wheel
    public float MaxBreakTorque; // maximum torque the motor can apply to wheel
    public float MaxSteeringAngle; // maximum steer angle the wheel can have
    public float MaxReverseTorque; // reverse torque
    public float DownForce; // down force

    public List<CarAxle> AxleInfos; // the information about each individual axle

    public float CurrentSpeed { get { return rgbd == null ? 0 : rgbd.velocity.magnitude * Speed_Multipler; } }

    public CarParticleController ParticleController;

    public AudioSource MotorAudioSource;
    public AudioSource HitAudioSource;

    private Rigidbody rgbd;

    private void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        Car = GetComponent<Car>();

        foreach (CarAxle axleInfo in AxleInfos)
        {
            axleInfo.Initialize();
        }
    }

    private void Update()
    {
        MotorAudioSource.volume = GameManager.CheckState(State.Pause) ? 0 : 1;
    }

    public void Move(float steering, float accel, float footbrake, float handbrake)
    {
        //Sanitize input values
        steering = Mathf.Clamp(steering, -1, 1);
        accel = Mathf.Clamp(accel, 0, 1);
        footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);

        MotorAudioSource.pitch = 0.4f;
        Lights.Reset();

        if (accel > 0)
        {
            MotorAudioSource.pitch = Mathf.Lerp(0.7f, 1.5f, CurrentSpeed / (TopSpeed / 2));
        }

        var steerAngle = steering * MaxSteeringAngle;
        foreach (CarAxle axleInfo in AxleInfos)
        {
            axleInfo.UpdateWheelTerrain();

            //Steering
            if (axleInfo.Steering)
            {
                float steerRandom = 0;
                if (Car.Health / Car.MaxHealth < 0.5f)
                {
                    float destructionFactor = 0.5f - Car.Health / Car.MaxHealth;
                    steerRandom = UnityEngine.Random.Range(-3, 3) * destructionFactor;
                }

                foreach (var wheel in axleInfo.Wheels)
                {
                    wheel.Wheel.steerAngle = steerAngle + steerRandom;
                }
            }

            //Driving
            if (axleInfo.Motor)
            {
                Car.AddFuel(-FuelBaseConsuption * accel);

                foreach (var wheel in axleInfo.Wheels)
                {
                    switch (wheel.WheelTerrain)
                    {
                        case WheelTerrainType.Sand:
                            ParticleController.EmitSandParticles();
                            wheel.Wheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionSand;
                            break;
                        case WheelTerrainType.SandRoad:
                            ParticleController.EmitSandRoadParticles();
                            wheel.Wheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionSandRoad;
                            break;
                        case WheelTerrainType.Asfalt:
                            wheel.Wheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionAsfalt;
                            break;
                        case WheelTerrainType.Grass:
                            ParticleController.EmitGrassParticles();
                            wheel.Wheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionGrass;
                            break;
                        case WheelTerrainType.Ice:
                            wheel.Wheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionIce;
                            break;
                        default:
                            wheel.Wheel.motorTorque = accel * MaxMotorTorque * axleInfo.TractionDefault;
                            break;
                    }
                }
            }

            if (CurrentSpeed > 10)
            {
                float driftValue = Vector3.Dot(rgbd.velocity, transform.forward);   

                if (driftValue < 4f)
                {
                    var force = SkidMarkForcePerSpeed();
                    Car.AddTires(-force * 0.035f);

                    foreach (var wheel in axleInfo.Wheels)
                    {
                        Track.Instance.SkidMarks.AddSkidMarks(wheel.Wheel.transform.position, force);
                    }
                }
            }

            //Breaking
            if (CurrentSpeed > 3 && Vector3.Angle(transform.forward, rgbd.velocity) < 50f)
            {
                if (footbrake > 0)
                {
                    Lights.ShowBreakLight();
                    var force = SkidMarkForcePerSpeed();
                    Car.AddTires(-force * 0.045f);

                    foreach (var wheel in axleInfo.Wheels)
                    {
                        Track.Instance.SkidMarks.AddSkidMarks(wheel.Wheel.transform.position, force);
                    }
                }

                foreach (var wheel in axleInfo.Wheels)
                {
                    wheel.Wheel.brakeTorque = MaxBreakTorque * footbrake;
                }
            }
            else if (footbrake > 0)
            {
                foreach (var wheel in axleInfo.Wheels)
                {
                    wheel.Wheel.brakeTorque = 0f;
                }

                if (axleInfo.Motor)
                {
                    foreach (var wheel in axleInfo.Wheels)
                    {
                        wheel.Wheel.motorTorque = -MaxReverseTorque * footbrake;
                        MotorAudioSource.pitch = 0.65f;
                    }

                    Car.AddFuel(-FuelBaseConsuption * Mathf.Abs(footbrake));
                }
            }

            if (handbrake > 0f)
            {
                var hbTorque = handbrake * MaxBreakTorque;

                foreach (var wheel in axleInfo.Wheels)
                {
                    wheel.Wheel.brakeTorque = hbTorque;
                }
            }

            if (footbrake == 0 && handbrake == 0)
            {
                foreach (var wheel in axleInfo.Wheels)
                {
                    wheel.Wheel.brakeTorque = 0f;
                }
            }

            foreach (var wheel in axleInfo.Wheels)
            {
                ApplyLocalPositionToVisuals(wheel.Wheel);
            }

            if (axleInfo.AddDownForce == true)
            {
                AddDownForce(axleInfo);
            }

            axleInfo.UpdateTireHealth(Car.Tires / Car.MaxTires);
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
            if (axle.Motor)
            {
                motorWheelCount += axle.Wheels.Count;

                foreach (var wheel in axle.Wheels)
                {
                    if (wheel.WheelTerrain != WheelTerrainType.Asfalt && wheel.WheelTerrain != WheelTerrainType.SandRoad)
                    {
                        offRoadTerrain++;
                    }
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
            var newVelocity = (maxSpeed / Speed_Multipler) * rgbd.velocity.normalized;
            rgbd.velocity = new Vector3(newVelocity.x, rgbd.velocity.y, newVelocity.z);
        }
    }

    // this is used to add more grip in relation to speed
    private void AddDownForce(CarAxle axleInfo)
    {
        foreach (var wheel in axleInfo.Wheels)
        {
            wheel.Wheel.attachedRigidbody.AddForce(-transform.up * DownForce * wheel.Wheel.attachedRigidbody.velocity.magnitude);
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.CheckState(State.Game) == false)
        {
            return;
        }

        if (collision.collider.gameObject.layer == (int)TrackMask.TrackObjects)
        {
            var trackObject = collision.collider.gameObject.GetComponent<TrackObject>();

            if (trackObject != null && trackObject.SoftCollision == false)
            {
                Car.AddHealth(-rgbd.velocity.magnitude * CrashObjectsDamageMultiplier);
                PlayHitSound();
            }
        }

        if (collision.collider.gameObject.layer == (int)TrackMask.CarBody)
        {
            var rigidbody = collision.collider.gameObject.GetComponentInParent<Rigidbody>();

            if (rigidbody != null)
            {
                var crashMagnitude = -(rgbd.velocity - rigidbody.velocity).magnitude;

                if (rgbd.velocity.magnitude < rigidbody.velocity.magnitude)
                {
                    rigidbody.GetComponent<Car>().AddHealth(crashMagnitude * CrashFasterCarDamageMultiplier);
                    Car.AddHealth(crashMagnitude * CrashSlowerCarDamageMultiplier);
                }
                else
                {
                    rigidbody.GetComponent<Car>().AddHealth(crashMagnitude * CrashSlowerCarDamageMultiplier);
                    Car.AddHealth(crashMagnitude * CrashFasterCarDamageMultiplier);
                }

                PlayHitSound();
            }
        }
    }

    private void PlayHitSound()
    {
        HitAudioSource.volume = Mathf.Lerp(0.1f, 1.5f, rgbd.velocity.magnitude / 10);
        HitAudioSource.Play();
    }
}
