using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pathfind;

public class plugindemo : MonoBehaviour
{
    //refrences
    private iamryan iar;

    //destinations
    public List<GameObject> destinaitons;
    public int currdestination;
    private GameObject rei;

    public bool candeliver = false;

    void Start()
    {
        //set refrences and initlise
        iar = this.gameObject.GetComponent<iamryan>();
        iar.whenFin = whenfinished();
        rei = GameObject.Find("PLAYER_rei");
        deliver();
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
        Debug.Log("done");


         //allows the script to reinitlise the direction the drone was traveling in 
         Path.currenttdirinit = true;

        if (currdestination == 0)
        {
            if (destinaitons[currdestination] == rei)
            {
                yield return new WaitForSeconds(0.25f);
                iar.source.GetComponent<drone>().drop(0);
                yield return new WaitForSeconds(1.0f);

                currdestination = 1;

                iar.recalculate();
                iar.movment = true;
            }
        }
        else
        {
            candeliver = true;
        }

        //update and move again
        iar.destination = destinaitons[currdestination];
        iar.whenFin = whenfinished();

        yield return null;
    }
}
