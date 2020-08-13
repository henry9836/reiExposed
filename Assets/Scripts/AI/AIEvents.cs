using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIEvents : MonoBehaviour
{

    AIObject ai;

    public UnityEvent onDeath;
    public UnityEvent onStart;

    bool deathInvoked = false;

    private void Start()
    {
        ai = GetComponent<AIObject>();
        onStart.Invoke();
    }

    private void FixedUpdate()
    {
        if (ai.health <= 0.0f)
        {
            if (!deathInvoked)
            {
                onDeath.Invoke();
                deathInvoked = true;
            }
        }
    }

}
