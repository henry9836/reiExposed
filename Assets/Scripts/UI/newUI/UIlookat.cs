using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIlookat : MonoBehaviour
{
    public GameObject cam;

    void Update()
    {
        this.transform.LookAt(cam.transform.position);
        this.transform.Rotate((Vector3.up * 180));
    }
}
