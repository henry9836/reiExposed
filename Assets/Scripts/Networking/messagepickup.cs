using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class messagepickup : MonoBehaviour
{
    public enemydrop canvas;

    public bool flag = false;

    void Start()
    {
        if (flag == false)
        {
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<enemydrop>();
            gameObject.transform.parent = null;


            canvas.processMessage();
            flag = true;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (flag == false)
            {
                if (!canvas)
                {
                    canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<enemydrop>();

                    canvas.processMessage();
                    flag = true;
                }



            }

            canvas.messagesToShow++;
            Destroy(this.gameObject);

        }
    }
}
