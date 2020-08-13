using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heatwaveManger : MonoBehaviour
{
    public Material tmp;
    public Renderer mat;
    public GameObject bosshead;
    public Vector3 offset;

    private bool calc = false;
    void Start()
    {
        this.mat.material = tmp;
        mat.material.SetFloat("Vector1_724DA0FA", 1.0f);
        calc = false;
    }

    void Update()
    {
        if (calc == true)
        {
            this.transform.position = bosshead.transform.position + offset;
        }
    }

    public void bossenter()
    {
        mat.material.SetFloat("Vector1_724DA0FA", 0.0f);
        calc = true;
    }


}
