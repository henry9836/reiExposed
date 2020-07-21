using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class slotno : MonoBehaviour
{
    public int slotnumber;
    public float sizeoutta1 = 0.0f;

    public float large;
    public float smol;

    public bool growing = false;
    public bool shriking = false;

    public float speed = 2.0f;



    public IEnumerator togrow()
    {
        for (; sizeoutta1 < 1.0f; sizeoutta1 += Time.deltaTime * speed)
        {
            this.gameObject.transform.localScale = Vector3.Lerp(new Vector3(smol, smol, smol), new Vector3(large, large, large), sizeoutta1);
            if (growing != true)
            {
                yield break;
            }
            yield return null;
        }
        sizeoutta1 = 1.0f;
        this.gameObject.transform.localScale = new Vector3(large, large, large);

        yield return null;
    }

    public IEnumerator toungrow()
    {
        for (; sizeoutta1 > 0.0f; sizeoutta1 -= Time.deltaTime * speed)
        {
            this.gameObject.transform.localScale = Vector3.Lerp(new Vector3(smol, smol, smol), new Vector3(large, large, large), sizeoutta1);
            if (shriking != true)
            {
                yield break;
            }
            yield return null;
        }
        sizeoutta1 = 0.0f;
        this.gameObject.transform.localScale = new Vector3(smol, smol, smol);

        yield return null;
    }
}
