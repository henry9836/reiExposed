using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bosswall : MonoBehaviour
{
    private umbrella TU;
    private bool entered = false;

    private void Start()
    {
        TU = GameObject.FindGameObjectWithTag("Player").GetComponent<umbrella>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (entered == false)
            {
                entered = true;

                this.GetComponent<BoxCollider>().enabled = true;
                TU.bossroomtrigger();
            }

        }
    }
}
