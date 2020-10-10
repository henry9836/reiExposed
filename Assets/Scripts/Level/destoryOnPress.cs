using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destoryOnPress : MonoBehaviour
{

    public GameObject objectToDestroy;
    public bool destoryThisObject = true;
    public bool useSaveSystem = true;

    private void Start()
    {
        if (useSaveSystem)
        {
            if (SaveSystemController.getBoolValue(name))
            {
                trigger();
            }
        }
    }

    private void trigger()
    {
        if (useSaveSystem)
        {
            SaveSystemController.updateValue(name, true);
            SaveSystemController.saveDataToDisk();
        }
        Destroy(objectToDestroy);
        if (destoryThisObject)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            trigger();
        }
    }
}
