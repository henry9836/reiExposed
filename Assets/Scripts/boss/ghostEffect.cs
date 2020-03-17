using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostEffect : MonoBehaviour
{
    public GameObject particles;
    //public Mesh theMesh;

    public List<GameObject> body = new List<GameObject>() { };
    public List<Mesh> meshes = new List<Mesh>() { };
    public List<GameObject> ghostbody = new List<GameObject>() { };



    void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).tag == "body")
            {
                body.Add(this.gameObject.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < body.Count; i++)
        {
            meshes.Add(body[i].GetComponent<SkinnedMeshRenderer>().sharedMesh);

        }

        for (int i = 0; i < body.Count; i++)
        {
            GameObject tmp = Instantiate(particles, body[i].gameObject.transform);

            tmp.transform.parent = body[i].GetComponent<SkinnedMeshRenderer>().rootBone;
            var sh = tmp.GetComponent<ParticleSystem>().shape;
            sh.mesh = meshes[i];

            ghostbody.Add(tmp);
            tmp.AddComponent<MeshCollider>();
            tmp.GetComponent<MeshCollider>().convex = true;
            tmp.GetComponent<MeshCollider>().sharedMesh = meshes[i];

            tmp.name = tmp.name + i;
            


            body[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = null;

        }




        //tmp.GetComponent<ParticleSystem>().shape.mesh = theMesh;


    }

}
