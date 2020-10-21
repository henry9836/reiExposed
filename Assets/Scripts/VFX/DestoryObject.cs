using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObject : MonoBehaviour
{

    public bool manualTrigger = false;
    public float timeTillDeath = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!manualTrigger)
        {
            StartCoroutine(destroy());
        }
    }

    public void Trigger()
    {
        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(timeTillDeath);
        Destroy(gameObject);
    }

}
