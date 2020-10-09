using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTriggerEventBox : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;
    public UnityEvent onStay;

    private void OnTriggerEnter(Collider other)
    {
        onEnter.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        onStay.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke();
    }

}
