using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float speed;
    private int currentpos = 0;
    private List<Vector3> positions = new List<Vector3>() { };
    private ballmaster BM;
    private bool stop = false;

    void Start()
    {
        BM = this.transform.parent.GetComponent<ballmaster>();
        positions = BM.positions;
        speed = BM.speed;
    }

    void Update()
    {
        if (stop == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, positions[currentpos], speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, positions[currentpos]) < 0.1f)
            {
                if (currentpos < positions.Count - 1)
                {
                    currentpos++;
                }
                else
                {
                    StartCoroutine(waitabit());
                }
            }
        }
    }

    public IEnumerator waitabit()
    {
        stop = true;
        BM.stop = true;
        int tmpsib = this.transform.GetSiblingIndex();
        if (tmpsib <= 0)
        {
            tmpsib = BM.transform.childCount - 1;
        }
        else
        {
            tmpsib--;
        }

        GameObject prevBall = BM.transform.GetChild(tmpsib).transform.gameObject; 


        while (Vector3.Distance(BM.transform.position, prevBall.transform.position) < BM.spawnDistance)
        {
            Debug.Log(Vector3.Distance(BM.transform.position, prevBall.transform.position));

            yield return null;
        }

        StartCoroutine(disbaleLine());

        currentpos = 0;
        transform.position = this.transform.parent.position;
        stop = false;

        yield return null;
    }

    public IEnumerator disbaleLine()
    {
        transform.GetComponent<TrailRenderer>().enabled = false;

        yield return new WaitForSeconds(0.5f);
        transform.GetComponent<TrailRenderer>().enabled = true;

    }
}
