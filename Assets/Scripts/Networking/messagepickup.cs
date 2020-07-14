using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class messagepickup : MonoBehaviour
{
    private enemydrop canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<enemydrop>();
        canvas.processMessage();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvas.messagesToShow++;
            Destroy(this.gameObject); 
        }
    }
}
