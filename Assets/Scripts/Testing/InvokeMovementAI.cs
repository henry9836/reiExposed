using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeMovementAI : MonoBehaviour
{
    public AIMovement movement;

    private void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            movement.setOverride(AIMovement.OVERRIDE.FULL_OVERRIDE);
            Debug.Log("Stopped");
        }
    }
}
