using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class materialDither : MonoBehaviour
{
    public Material reimat;
    public Material gunMat;
    private Transform cam;
    private Transform player;


    void Start()
    {
        cam = Camera.main.gameObject.transform;
        player = cam.transform.root.gameObject.transform;
    }

    void Update()
    {
        float dist = Vector3.Distance(cam.position, player.position);
        reimat.SetFloat("Vector1_A8AC28E2", dist);
        gunMat.SetFloat("Vector1_A8AC28E2", dist);
    }
}
