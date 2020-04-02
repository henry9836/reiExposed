using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public enum PLAYLIST_MODES
    {
        NONE,
        CALM,
        BATTLE,
    }

    public PLAYLIST_MODES startMode = PLAYLIST_MODES.CALM;
    public List<AudioClip> calmClips = new List<AudioClip>();
    public List<AudioClip> battleClips = new List<AudioClip>();


    private AudioSource audioSource;
    private PLAYLIST_MODES playMode;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        playMode = startMode;
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.Play();
    }


    public void startPlaylist(PLAYLIST_MODES mode)
    {
        playMode = mode;
        Stop();
        LoadClip();
    }

    public void StartCalm()
    {
        startPlaylist(PLAYLIST_MODES.CALM);
    }

    public void StartBattle()
    {
        startPlaylist(PLAYLIST_MODES.BATTLE);
    }


    public void LoadClip()
    {

        switch (playMode)
        {
            case PLAYLIST_MODES.CALM:
                {
                    audioSource.clip = calmClips[Random.Range(0, calmClips.Count)];
                    break;
                }
            case PLAYLIST_MODES.BATTLE:
                {
                    audioSource.clip = battleClips[Random.Range(0, battleClips.Count)];
                    break;
                }
            default:
                {
                    break;
                }
        }
        Resume();
    }


    private void FixedUpdate()
    {
        if (!audioSource.isPlaying)
        {
            //Pick a random clip based on playlist mode
            LoadClip();
        }
    }


}
