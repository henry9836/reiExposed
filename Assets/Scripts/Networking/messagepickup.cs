using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class messagepickup : MonoBehaviour
{
    private GameObject canvas;

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("MainCanvas");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canvas.GetComponent<enemydrop>().enemyiskil();
            Destroy(this.gameObject); 
        }
    }
}
