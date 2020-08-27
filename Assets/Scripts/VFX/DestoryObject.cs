using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObject : MonoBehaviour
{

    public float timeTillDeath = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(timeTillDeath);
        Destroy(gameObject);
    }

}
