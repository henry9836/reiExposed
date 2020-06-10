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
        GameObject.FindGameObjectWithTag("Boss").GetComponent<ReprisialOfFlameController>().sleepOveride = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (entered == false)
            {
                GameObject.FindGameObjectWithTag("Boss").GetComponent<ReprisialOfFlameController>().sleepOveride = false;

                entered = true;

                this.GetComponent<BoxCollider>().enabled = true;
                TU.bossroomtrigger();
            }

        }
    }
}
