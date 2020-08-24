using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshColliderupdate : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public MeshCollider collider;

    private Mesh colliderMesh;

    private void Start()
    {
        colliderMesh = new Mesh();
    }

    void Update()
    {
        meshRenderer.BakeMesh(colliderMesh);
        collider.sharedMesh = colliderMesh;
    }

}
