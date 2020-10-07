using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRepeatSFX : MonoBehaviour
{
    public AudioSource audio;

    private void Start()
    {
        if (audio == null)
        {
            audio = GetComponent<AudioSource>();
        }
    }

    public void Play()
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }
    }
}
