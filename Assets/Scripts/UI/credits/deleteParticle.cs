using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class deleteParticle : MonoBehaviour
{

    public List<AudioClip> explodeSounds = new List<AudioClip>();

    private float timer = 25.0f;

    private float effectpersent;
    private GameObject effectsref;
    

    void Start()
    {
        GameObject manager = GameObject.Find("Canvas");

        if (manager.GetComponent<creditsManager>().score % 10 == 0)
        {
            StartCoroutine(manager.GetComponent<creditsManager>().scorepading());
        }

        GetComponent<AudioSource>().clip = explodeSounds[Random.Range(0, explodeSounds.Count)];
        GetComponent<AudioSource>().Play();
        effectsref = GameObject.Find("Scene PostProcess");
        StartCoroutine(effects());
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public IEnumerator effects()
    {
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * 10.0f)
        {
            effectpersent = i;
            updatevalues(effectpersent);


            yield return null;
        }

        effectpersent = 1.0f;
        updatevalues(effectpersent);


        for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime * 10.0f)
        {
            effectpersent = i;
            updatevalues(effectpersent);


            yield return null;
        }
        effectpersent = 0.0f;
        updatevalues(effectpersent);

        yield return null;

    }

    void updatevalues(float effectammount)
    {

        ChromaticAberration tmp;
        if (effectsref.GetComponent<Volume>().profile.TryGet(out tmp))
        {
            tmp.intensity.value = effectammount;
        }

        LensDistortion tmp2;
        if (effectsref.GetComponent<Volume>().profile.TryGet(out tmp2))
        {
            tmp2.intensity.value = effectammount * 0.4f;
        }
    }
}
