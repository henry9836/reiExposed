using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallController : MonoBehaviour
{

    public float travelSpeed = 10.0f;

    public void fullSpeedAheadCaptain()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * travelSpeed, Space.World);
    }
}
