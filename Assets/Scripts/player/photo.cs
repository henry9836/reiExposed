using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photo : MonoBehaviour
{
    public GameObject enemy;
    public LayerMask body;

    public List<int> toremove = new List<int>() {};

    void Update()
    {
        if (Input.GetButtonDown("TakePhoto"))
        {
           


            for (int i = 0; i < enemy.GetComponent<ghostEffect>().ghostbody.Count; i++)
            {



                //if (Physics.Linecast(this.gameObject.transform.position, enemy.GetComponent<ghostEffect>().body[i].GetComponent<SkinnedMeshRenderer>().bounds.center))
                //{
                //    Debug.DrawLine(this.gameObject.transform.position, enemy.GetComponent<ghostEffect>().body[i].GetComponent<SkinnedMeshRenderer>().bounds.center, Color.red, 5.0f);

                //}
                //else
                //{
                //    Debug.Log("draw");
                //    Debug.DrawLine(this.gameObject.transform.position, enemy.GetComponent<ghostEffect>().body[i].GetComponent<SkinnedMeshRenderer>().bounds.center, Color.white, 5.0f);
                //    //remove(i);
                //}

                Vector3 raycastDir = enemy.GetComponent<ghostEffect>().body[i].GetComponent<SkinnedMeshRenderer>().bounds.center - this.gameObject.transform.position;


                RaycastHit hit;
                if (Physics.Raycast(this.gameObject.transform.position, raycastDir, out hit, Mathf.Infinity, body))
                {
                    //Debug.DrawRay(this.gameObject.transform.position, raycastDir, Color.red, 5.0f);

                    if (hit.collider.name == enemy.GetComponent<ghostEffect>().ghostbody[i].name)
                    {
                        //Debug.DrawLine(transform.position, hit.point, Color.yellow, 5.0f);
                        toremove.Add(i);
                    }

                }
            }

            while (toremove.Count > 0)
            {
                remove(toremove[toremove.Count - 1]);
                toremove.RemoveAt(toremove.Count - 1);
            }

        }
    }


    void remove(int i)
    {
        enemy.GetComponent<ghostEffect>().body[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = enemy.GetComponent<ghostEffect>().meshes[i];
        Destroy(enemy.GetComponent<ghostEffect>().ghostbody[i].GetComponent<ParticleSystem>());
    }
}
