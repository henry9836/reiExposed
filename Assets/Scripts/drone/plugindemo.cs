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
    public GameObject rei;
    public GameObject drone;
    public GameObject introUIResue;
    public GameObject gameMNGR;

    public bool candeliver = false;


    public int todrop = 0;
    void Start()
    {
        //set refrences and initlise
        iar = this.gameObject.GetComponent<iamryan>();
        iar.whenFin = whenfinished();
        deliver();
        gameMNGR = GameObject.Find("GameManager");
    }

    void LateUpdate()
    {




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
                    gameMNGR.GetComponent<GameManager>().stopPlayer(true);
                    drone.transform.position = rei.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
                    this.GetComponent<iamryan>().movment = false;
                    yield return new WaitForSeconds(0.5f);

                    StartCoroutine(picmeup());

                    yield return null;
                }


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

    public IEnumerator picmeup()
    {

        introUIResue.transform.GetChild(0).gameObject.SetActive(true);

        //fade in
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime)
        {
            introUIResue.transform.GetChild(0).GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, i);
            yield return null;
        }
        introUIResue.transform.GetChild(0).GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        //move player and drone
        rei.transform.position = new Vector3(-109.36f, 36.968f, 85.34f);
        drone.transform.position = this.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
        this.GetComponent<iamryan>().movment = true;

        yield return null;

        //fade out
        for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime)
        {
            introUIResue.transform.GetChild(0).GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, i);
            yield return null;

        }
        introUIResue.transform.GetChild(0).GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        introUIResue.transform.GetChild(0).gameObject.SetActive(false);
        gameMNGR.GetComponent<GameManager>().stopPlayer(false);
        yield return null;
    }
}
