using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostEffect : MonoBehaviour
{
    public GameObject particles;
    public Mesh theMesh;

    void Start()
    {
        GameObject tmp = Instantiate(particles, gameObject.transform);

        //tmp.GetComponent<ParticleSystem>().shape.mesh = theMesh;


        var sh = tmp.GetComponent<ParticleSystem>().shape;
        sh.mesh = theMesh;
    }

}
