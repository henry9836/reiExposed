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
            Debug.Log(canvas);
            Debug.Log(canvas.GetComponent<enemydrop>());
            canvas.GetComponent<enemydrop>().enemyiskil();
            Destroy(this.gameObject); 
        }
    }
}
