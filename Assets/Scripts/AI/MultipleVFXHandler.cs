using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleVFXHandler : MonoBehaviour
{
    public List<ParticleSystem> particles = new List<ParticleSystem>();

    public void Play(int i)
    {
        particles[i].gameObject.SetActive(true);
        particles[i].Play();
    }
    public void Stop(int i)
    {
        particles[i].Stop();
        particles[i].gameObject.SetActive(false);
    }

    public void StopAll()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
            particles[i].gameObject.SetActive(false);
        }
    }

}
