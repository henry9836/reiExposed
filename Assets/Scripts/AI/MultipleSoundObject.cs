using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MultipleSoundObject : MonoBehaviour
{
    public List<AudioClip> clips = new List<AudioClip>();
    List<AudioSource> audioSources = new List<AudioSource>();

    private void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        for (int s = 0; s < sources.Length; s++)
        {
            audioSources.Add(sources[s]);
        }
    }

    public void PlayRandomRange(int x, int y)
    {
        Play(clips[Random.Range(x, y)]);
    }
    public void PlayRandom()
    {
        Play(clips[Random.Range(0, clips.Count)]);
    }

    public void Play(int i)
    {
        Play(clips[i]);
    }
    public void Play(AudioClip clip)
    {
        for (int i = 0; i < audioSources.Count - 1; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].PlayOneShot(clip);
                return;
            }
        }

        audioSources[0].PlayOneShot(clip);

    }

}
