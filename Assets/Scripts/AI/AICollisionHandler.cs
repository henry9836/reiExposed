using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICollisionHandler : MonoBehaviour
{

    public AIObject aiObject;

    // Start is called before the first frame update
    public virtual void Start()
    {
        //Disable AIObjects built-in dectection
        aiObject = GetComponent<AIObject>();
        aiObject.handleCollision = false;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("A Collison Was Detected");
    }
}
