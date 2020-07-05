using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinematicMenu : MonoBehaviour
{
    public GameObject target;
    public Camera Cam;
    private float theta = 0.0f;
    public float speed = 1.0f;
    public float height = 2.0f;
    public float circleSize = 1.0f;
    public float center = 0.0f;
    public Vector3 targetoffset;

    void Update()
    {
        theta += (Time.deltaTime * speed);
        Cam.GetComponent<Transform>().localPosition = new Vector3(Mathf.Sin(theta) * circleSize, height, Mathf.Cos(theta) * circleSize);

        Cam.transform.LookAt(new Vector3(target.transform.localPosition.x + targetoffset.x, target.transform.localPosition.y + targetoffset.y, target.transform.localPosition.z + targetoffset.z), Vector3.up);
        Cam.transform.Rotate(0.0f, center, 0.0f);
    }
}
