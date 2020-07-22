using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    public IEnumerator togrow()
    {
        for (; sizeoutta1 < 1.0f; sizeoutta1 += Time.deltaTime * speedgrow)
        {
            if (growing != true)
            {
                yield break;
            }
            this.gameObject.transform.localScale = Vector3.Lerp(new Vector3(smol, smol, smol), new Vector3(large, large, large), sizeoutta1);
            yield return null;
        }
        sizeoutta1 = 1.0f;
        this.gameObject.transform.localScale = new Vector3(large, large, large);
        growing = false;
        yield return null;
    }

    public IEnumerator toungrow()
    {
        for (; sizeoutta1 > 0.0f; sizeoutta1 -= Time.deltaTime * speedshrink)
        {
            if (shriking != true)
            {
                yield break;
            }
            this.gameObject.transform.localScale = Vector3.Lerp(new Vector3(smol, smol, smol), new Vector3(large, large, large), sizeoutta1);
            yield return null;
        }
        sizeoutta1 = 0.0f;
        this.gameObject.transform.localScale = new Vector3(smol, smol, smol);
        shriking = false;
        yield return null;
    }
}
