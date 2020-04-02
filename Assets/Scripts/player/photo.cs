using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photo : MonoBehaviour
{
    public GameObject enemy;
    public LayerMask body;

    public List<int> toremove = new List<int>() {};

    public Animator animator;

    public bool cantake = true;
    public AudioClip takephotoSFX;

    private GameObject flash;
    

    private void Start()
    {
        if (!enemy)
        {
            GameObject.FindGameObjectWithTag("Boss");
        }
        flash = this.gameObject.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Input.GetButtonDown("TakePhoto") && cantake == true)
        {
            animator.SetTrigger("Photo");
        }
    }



    public void take()
    {

        animator.gameObject.GetComponent<AudioSource>().PlayOneShot(takephotoSFX);
        StartCoroutine(camflash());

        for (int i = 0; i < enemy.GetComponent<ghostEffect>().ghostbody.Count; i++)
        {
            Vector3 raycastDir = enemy.GetComponent<ghostEffect>().body[i].GetComponent<SkinnedMeshRenderer>().bounds.center - this.gameObject.transform.position;


            RaycastHit hit;
            if (Physics.Raycast(this.gameObject.transform.position, raycastDir, out hit, Mathf.Infinity, body))
            {
                if (hit.collider.name == enemy.GetComponent<ghostEffect>().ghostbody[i].name)
                {
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

    void remove(int i)
    {
        if (enemy.GetComponent<ghostEffect>().ghostbody[i].GetComponent<ParticleSystem>())
        {
            enemy.GetComponent<ghostEffect>().deactivatedghostbody.Add(enemy.GetComponent<ghostEffect>().ghostbody[i]);
            enemy.GetComponent<ghostEffect>().body[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = enemy.GetComponent<ghostEffect>().meshes[i];
            Destroy(enemy.GetComponent<ghostEffect>().ghostbody[i].GetComponent<ParticleSystem>());
        }

    }

    IEnumerator camflash()
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(0.001f);
        flash.SetActive(false);


        yield return null;
    }
}
