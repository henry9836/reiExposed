using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryParticle : MonoBehaviour
{

    //This script destorys a object once all of it's child particles have stopped
    //TO USE THIS NON OF THE PARTICLE SYSTEM CAN BE SET TO LOOP

    List<ParticleSystem> particles = new List<ParticleSystem>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ParticleSystem>())
            {
                particles.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            if (particles[i].isStopped)
            {
                Destroy(gameObject);
            }
        }
    }


}
