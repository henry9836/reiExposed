using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class deleteParticle : MonoBehaviour
{
    private float timer = 25.0f;

    private float effectpersent;
    private GameObject effectsref;

    void Start()
    {
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
            effectsref.GetComponent<ChromaticAberration>().intensity.value = effectpersent;
            effectsref.GetComponent<LensDistortion>().intensity.value = effectpersent * 0.4f;

            yield return null;
        }

        effectpersent = 1.0f;
        effectsref.GetComponent<ChromaticAberration>().intensity.value = effectpersent;
        effectsref.GetComponent<LensDistortion>().intensity.value = effectpersent * 0.4f;

        for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime * 10.0f)
        {
            effectpersent = i;
            effectsref.GetComponent<ChromaticAberration>().intensity.value = effectpersent;
            effectsref.GetComponent<LensDistortion>().intensity.value = effectpersent * 0.4f;

            yield return null;
        }
        effectpersent = 0.0f;
        effectsref.GetComponent<ChromaticAberration>().intensity.value = effectpersent;
        effectsref.GetComponent<LensDistortion>().intensity.value = effectpersent * 0.4f;


        yield return null;

    }
}
