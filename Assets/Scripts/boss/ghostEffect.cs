using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ghostEffect : MonoBehaviour
{
    public GameObject particles;
    //public Mesh theMesh;

    public List<GameObject> body = new List<GameObject>() { };
    public List<Mesh> meshes = new List<Mesh>() { };
    public List<GameObject> ghostbody = new List<GameObject>() { };
    public List<GameObject> deactivatedghostbody = new List<GameObject>() { };


    public GameObject UIghost;
    public GameObject UIHP;

    public float ghostpersent;





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

    }

    void Update()
    {
        ghostpersent = 1.0f - ((float)deactivatedghostbody.Count / (float)ghostbody.Count);
        UIghost.GetComponent<Image>().fillAmount = ghostpersent;
        UIHP.GetComponent<Image>().fillAmount = (this.gameObject.transform.GetComponent<BossController>().health / this.gameObject.transform.GetComponent<BossController>().maxHealth);



        if ((float)deactivatedghostbody.Count / (float)ghostbody.Count > 0.92f && (float)deactivatedghostbody.Count / (float)ghostbody.Count < 0.99999999f)
        {
            finish();
            Debug.Log("complete");
        }
    }

    void finish()
    {
        for (int i = 0; i < ghostbody.Count; i++)
        {
            if (ghostbody[i].GetComponent<ParticleSystem>())
            {
                deactivatedghostbody.Add(ghostbody[i]);
                body[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = meshes[i];
                Destroy(ghostbody[i].GetComponent<ParticleSystem>());
            }
        }
    }

}
