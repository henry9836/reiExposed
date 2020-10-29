using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinmaticCamControler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("Play");
            Debug.Log("playing animation");
        }
    }
}
