using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIlookat : MonoBehaviour
{
    private GameObject cam;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
    }

    void Update()
    {
        this.transform.LookAt(cam.transform.position);
        this.transform.Rotate((Vector3.up * 180));
    }
}
