using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slotno : MonoBehaviour
{
    public int slotnumber;
    public float sizeoutta1 = 0.0f;

    public float large;
    public float smol;

    public bool growing = false;
    public bool shriking = false;

    public float speedgrow = 7.0f;
    public float speedshrink = 7.0f;

    private float shaderammountmin = 1.0f;
    private float shaderammountmax = 0.0f;
    private Vector4 colormin = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
    private Vector4 colormax = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

    public bool touchMat = true;

    private void Start()
    {
        if (touchMat)
        {
            this.gameObject.GetComponent<Image>().material = new Material(this.gameObject.GetComponent<Image>().material);
            this.gameObject.GetComponent<Image>().material.SetColor("_Color", colormin);
            this.gameObject.GetComponent<Image>().material.SetFloat("_Power", shaderammountmin);
        }

    }


    public IEnumerator togrow()
    {
        if (touchMat)
        {
            for (; sizeoutta1 < 1.0f; sizeoutta1 += Time.deltaTime * speedgrow)
            {
                if (growing != true)
                {
                    yield break;
                }
                this.gameObject.GetComponent<Image>().material.SetColor("_Color", Vector4.Lerp(colormin, colormax, sizeoutta1));

                this.gameObject.GetComponent<Image>().material.SetFloat("_Power", Mathf.Lerp(shaderammountmin, shaderammountmax, sizeoutta1));
                this.gameObject.transform.localScale = Vector3.Lerp(new Vector3(smol, smol, smol), new Vector3(large, large, large), sizeoutta1);
                yield return null;
            }
            sizeoutta1 = 1.0f;
            this.gameObject.GetComponent<Image>().material.SetFloat("_Power", shaderammountmax);
            this.gameObject.GetComponent<Image>().material.SetColor("_Color", colormax);
            this.gameObject.transform.localScale = new Vector3(large, large, large);
            growing = false;
            yield return null;
        }
        yield return null;

    }

    public IEnumerator toungrow()
    {
        if (touchMat)
        {
            for (; sizeoutta1 > 0.0f; sizeoutta1 -= Time.deltaTime * speedshrink)
            {
                if (shriking != true)
                {
                    yield break;
                }
                this.gameObject.GetComponent<Image>().material.SetColor("_Color", Vector4.Lerp(colormin, colormax, sizeoutta1));

                this.gameObject.GetComponent<Image>().material.SetFloat("_Power", Mathf.Lerp(shaderammountmin, shaderammountmax, sizeoutta1));
                this.gameObject.transform.localScale = Vector3.Lerp(new Vector3(smol, smol, smol), new Vector3(large, large, large), sizeoutta1);
                yield return null;
            }
            sizeoutta1 = 0.0f;
            this.gameObject.GetComponent<Image>().material.SetFloat("_Power", shaderammountmin);

            this.gameObject.GetComponent<Image>().material.SetColor("_Color", colormin);

            this.gameObject.transform.localScale = new Vector3(smol, smol, smol);
            shriking = false;
            yield return null;
            }

        yield return null;
    }
}
