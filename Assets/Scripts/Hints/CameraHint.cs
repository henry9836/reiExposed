using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHint : MonoBehaviour
{
    public ClueController clueCtrl;

    public bool isQRCode = false;
    
    private void FixedUpdate()
    {
        if (!isQRCode)
        {
            if (clueCtrl.cluesCollected.Count > 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (clueCtrl.qrFound)
            {
                Destroy(gameObject);
            }
        }
    }

}
