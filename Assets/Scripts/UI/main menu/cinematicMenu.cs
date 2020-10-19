using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinematicMenu : MonoBehaviour
{
    public GameObject rotateabout;
    public GameObject target;
    public Camera Cam;
    private float theta = 0.0f;
    public float speed = 1.0f;
    public float height = 2.0f;
    public float circleSize = 1.0f;
    public float center = 0.0f;
    public Vector3 targetoffset;

    [Header("rotate on these axis")]
    public bool x;
    public bool y;
    public bool z;


    void Update()
    {
        theta += (Time.deltaTime * speed);
        Vector3 newpos = Vector3.zero;
        if (x == true)
        {
            newpos += new Vector3(Mathf.Sin(theta) * circleSize, 0, 0);
        }
        else
        {
            newpos += new Vector3(height, 0, 0);
        }

        if (y == true)
        {
            if (x == true)
            {
                newpos += new Vector3(0, Mathf.Cos(theta) * circleSize, 0);
            }
            else
            {
                newpos += new Vector3(0, Mathf.Sin(theta) * circleSize, 0);
            }
        }
        else
        {
            newpos += new Vector3(0, height, 0);
        }

        if (z == true)
        {
            newpos += new Vector3(0, 0, Mathf.Cos(theta) * circleSize);
        }
        else
        {
            newpos += new Vector3(0, 0, height);
        }
        Cam.GetComponent<Transform>().localPosition = newpos + rotateabout.transform.localPosition; 

        Cam.transform.LookAt(new Vector3(target.transform.position.x + targetoffset.x, target.transform.position.y + targetoffset.y, target.transform.position.z + targetoffset.z), Vector3.up);
        Cam.transform.Rotate(0.0f, center, 0.0f);
    }
}
