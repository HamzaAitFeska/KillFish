using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStyleActivator : MonoBehaviour
{
    public ParticleSystem theParticle;

    public void ParticleActivator()
    {
        theParticle.Play();
        print("hola hehe");
    }
}
