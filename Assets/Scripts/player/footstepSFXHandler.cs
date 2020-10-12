using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstepSFXHandler : MonoBehaviour
{
    public List<AudioClip> stepSounds = new List<AudioClip>();
    public AudioSource audio;
    
    private float timeSinceLastPlay = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        float timeSinceLastInvoke = Time.time - timeSinceLastPlay;
        timeSinceLastPlay = Time.time;
        if (timeSinceLastInvoke > 0.1f)
        {
            if (other.tag == "Untagged")
            {
                Debug.Log($"AUDIO: {other.name} | {timeSinceLastInvoke}");
                audio.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Count)]);
            }
        }
    }

}
