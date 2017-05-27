using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParticleController : MonoBehaviour
{
    public ParticleSystem SandParticles;
    public ParticleSystem GrassParticles;
    public ParticleSystem Smoke;
    public ParticleSystem SandRoadParticles;

    private Car car;
    private CarController carController;

    public void Awake()
    {
        car = GetComponentInParent<Car>();
        carController = car.CarController;

        SandParticles = Instantiate(SandParticles);
        SandParticles.transform.SetParent(transform, false);

        GrassParticles = Instantiate(GrassParticles);
        GrassParticles.transform.SetParent(transform, false);

        Smoke = Instantiate(Smoke);
        Smoke.transform.SetParent(transform, false);

        SandRoadParticles = Instantiate(SandRoadParticles);
        SandRoadParticles.transform.SetParent(transform, false);
    }

    private bool sandEmitting;
    public void EmitSandParticles()
    {
        if (carController.CurrentSpeed > 1)
        {
            if (SandParticles.isPlaying == false)
            {
                SandParticles.Play();
            }

            ParticleSystem.MainModule mainModule = SandParticles.main;
            mainModule.startSize = Mathf.Min(2, carController.CurrentSpeed / 10);

            sandEmitting = true;
        }
    }

    private void CleanSandEmitter()
    {
        if (!sandEmitting && SandParticles.isPlaying)
        {
            SandParticles.Stop();
        }

        sandEmitting = false;
    }

    private bool grassEmitting;
    public void EmitGrassParticles()
    {
        if (carController.CurrentSpeed > 1)
        {
            if (GrassParticles.isPlaying == false)
            {
                GrassParticles.Play();
            }

            ParticleSystem.MainModule mainModule = GrassParticles.main;
            mainModule.startSize = Mathf.Min(0.2f, carController.CurrentSpeed / 10);

            grassEmitting = true;
        }
    }

    private void CleanGrassEmmiter()
    {
        if (!grassEmitting && GrassParticles.isPlaying)
        {
            GrassParticles.Stop();
        }

        grassEmitting = false;
    }


    public void EmitSmoke()
    {
        if (car.Health < car.MaxHealth)
        {
            if (Smoke.isPlaying == false)
            {
                Smoke.Play();
            }

            ParticleSystem.EmissionModule emissionModule = Smoke.emission;
            emissionModule.rateOverTime = Mathf.Max(0, (1 - car.Health / car.MaxHealth) * 30);
        }
        else
        {
            Smoke.Stop();
        }
    }


    private bool sandRoadEmitting;
    public void EmitSandRoadParticles()
    {
        if (carController.CurrentSpeed > 1)
        {
            if (SandRoadParticles.isPlaying == false)
            {
                SandRoadParticles.Play();
            }

            ParticleSystem.MainModule mainModule = SandRoadParticles.main;
            mainModule.startSize = Mathf.Min(2, carController.CurrentSpeed / 10);

            sandRoadEmitting = true;
        }
    }

    private void CleanSandRoadEmitter()
    {
        if (!sandEmitting && SandRoadParticles.isPlaying)
        {
            SandRoadParticles.Stop();
        }

        sandRoadEmitting = false;
    }


    public void CleanEmmiters()
    {
        CleanSandEmitter();
        CleanSandRoadEmitter();
        CleanGrassEmmiter();
    }

    public void Update()
    {
        EmitSmoke();
    }
}
