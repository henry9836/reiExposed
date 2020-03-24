using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
    public UnityEvent triggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Something called {other.gameObject.name} came inside me");

        if (other.CompareTag("Player"))
        {
            triggerEvent.Invoke();
            Destroy(gameObject);
        }
    }


}
