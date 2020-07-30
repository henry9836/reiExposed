using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destoryOnPress : MonoBehaviour
{

    public GameObject objectToDestroy;
    public bool destoryThisObject = true;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(objectToDestroy);
        if (destoryThisObject)
        {
            Destroy(this.gameObject);
        }
    }
}
