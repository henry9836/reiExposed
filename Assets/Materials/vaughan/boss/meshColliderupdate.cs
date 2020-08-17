using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshColliderupdate : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public MeshCollider collider;

    void Update()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        collider.sharedMesh = null;
        collider.sharedMesh = colliderMesh;
    }

}
