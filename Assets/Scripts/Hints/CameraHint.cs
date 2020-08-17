using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHint : MonoBehaviour
{
    public ClueController clueCtrl;
    public GameObject monitorObject;

    public bool destoryWithObject = false;


    private void FixedUpdate()
    {
        if (!destoryWithObject)
        {
            if (clueCtrl.cluesCollected.Count > 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (destoryWithObject)
            {
                if (monitorObject == null) {
                    Destroy(gameObject);
                }
            }
        }
    }

}
