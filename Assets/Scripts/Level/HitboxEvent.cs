using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HitboxEvent : MonoBehaviour
{

    public UnityEvent OnEnter;
    public bool destoryOnEnter = true;

    private void OnTriggerEnter(Collider other)
    {
        OnEnter.Invoke();
        if (destoryOnEnter)
        {
            Destroy(gameObject);
        }
    }

}
