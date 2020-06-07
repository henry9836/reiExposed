using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePhone : MonoBehaviour
{
    private plugindemo drone;

    void Start()
    {
        drone = GameObject.Find("Save&Dronemanage").GetComponent<plugindemo>();
    }

    void Update()
    {

        if (drone.candeliver == true)
        {
            if (Input.GetKeyDown(KeyCode.Tab) == true)
            {
                drone.deliver();
            }
        }
    }

}
