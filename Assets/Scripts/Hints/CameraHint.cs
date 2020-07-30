using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHint : MonoBehaviour
{
    public ClueController clueCtrl;

    private void FixedUpdate()
    {
        if (clueCtrl.cluesCollected.Count > 0)
        {
            Destroy(gameObject);
        }
    }

}
