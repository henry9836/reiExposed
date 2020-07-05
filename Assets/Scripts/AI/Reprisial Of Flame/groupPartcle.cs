using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class groupPartcle : MonoBehaviour
{
    public List<ParticleSystem> particles = new List<ParticleSystem>();

    public void Play()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            if (!particles[i].isPlaying) {
                particles[i].Play();
                GetComponent<AudioSource>().Play();
            }
        }
    }

    public void Stop()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
    }

}
