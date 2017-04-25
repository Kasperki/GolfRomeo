using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem SandParticles;
    //public ParticleSystem Smoke;

    private CarController carController;

    public void Awake()
    {
        carController = GetComponentInParent<CarController>();
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

    public void CleanEmmiters()
    {
        CleanSandEmitter();
    }
}
