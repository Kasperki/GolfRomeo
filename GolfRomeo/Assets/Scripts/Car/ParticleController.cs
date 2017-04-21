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

    public void EmitSandParticles()
    {
        if (carController.CurrentSpeed > 20)
        {
            ParticleSystem.EmissionModule em = SandParticles.emission;
            em.enabled = true;
        }
    }

    public void Update()
    {
        ParticleSystem.EmissionModule em = SandParticles.emission;
        em.enabled = false;
    }
}
