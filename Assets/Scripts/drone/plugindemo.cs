using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pathfind;
using UnityEngine.UI;

public class plugindemo : MonoBehaviour
{
    //refrences
    private iamryan iar;

    //destinations
    public List<GameObject> destinaitons;
    public int currdestination;
    private GameObject rei;
    public GameObject drone;

    public bool candeliver = false;
    public bool holdRei = false;


    public int todrop = 0;
    void Start()
    {
        //set refrences and initlise
        iar = this.gameObject.GetComponent<iamryan>();
        iar.whenFin = whenfinished();
        rei = GameObject.FindGameObjectWithTag("Player");
        deliver();
    }

    void Update()
    {
        if (holdRei == true)
        {
            rei.transform.position = drone.transform.position;
        }

        if (candeliver == false)
        {
            float dist = Vector3.Distance(drone.transform.position, destinaitons[currdestination].transform.position);
            dist = Mathf.Sqrt((dist + 2));
            dist = Mathf.Clamp(dist, 2, 10);
            SaveSystemController.updateValue("dynamicedgesize", dist * 10.0f);
            SaveSystemController.updateValue("deets", dist);
        }
    }


    public void deliver()
    {
        currdestination = 0;
        candeliver = false;
        iar.destination = destinaitons[currdestination];
        iar.movfin1call = true;
    }



    //custom ienum that gets called from other script when the source reaches the destination
    public IEnumerator whenfinished()
    {
         //allows the script to reinitlise the direction the drone was traveling in 
         Path.currenttdirinit = true;

        if (currdestination == 0)
        {
            if (destinaitons[currdestination] == rei)
            {
                if (todrop != 999)
                {
                    yield return new WaitForSeconds(0.25f);
                    iar.source.GetComponent<drone>().drop(todrop);
                    yield return new WaitForSeconds(1.0f);
                }
                else // pick me up mum
                {
                    yield return new WaitForSeconds(0.25f);
                    holdRei = true;
                }


                currdestination = 1;

                iar.recalculate();
                iar.movment = true;
            }
        }
        else
        {
            candeliver = true;
            holdRei = false;

        }

        //update and move again
        iar.destination = destinaitons[currdestination];
        iar.whenFin = whenfinished();

        yield return null;
    }
}
