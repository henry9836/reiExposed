using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserController : MonoBehaviour
{
    public GameObject Explode;
    public GameObject Prepare;
    public float timeTillExplode = 1.0f;
    public Collider hitBox;
    public AudioClip explode;
    public AudioClip passive;
    public AudioSource audio;

    void Play(AudioClip clip)
    {
        audio.Stop();
        audio.clip = clip;
        audio.Play();
    }

    private void Start()
    { 
        Play(passive);
        Explode.SetActive(false);
        Prepare.SetActive(true);
        hitBox.enabled = false;
        StartCoroutine(ticktock());
    }

    IEnumerator ticktock()
    {
        yield return new WaitForSeconds(timeTillExplode);
        Play(explode);
        Explode.SetActive(true);
        Prepare.SetActive(false);
        hitBox.enabled = true;
        //Wait till end of audio clip
        while (audio.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }


}
