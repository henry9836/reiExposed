using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelPlayerArea : MonoBehaviour
{


    public float initalpushForce = 5.0f;
    public float pushIncrease = 0.1f;

    float pushForce = 1.0f;
    Vector3 outDir = Vector3.zero;
    movementController moveCtrl;

    private void Start()
    {
        pushForce = initalpushForce;
        moveCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<movementController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            outDir = (other.gameObject.transform.position - transform.position).normalized;
            Debug.Log("In Push Zone");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") {
            moveCtrl.forceMovement(outDir * pushForce);

            pushForce += pushIncrease * Time.deltaTime;
            Debug.Log("Push");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pushForce = initalpushForce;
            Debug.Log("Out Push Zone");
        }
    }

}
