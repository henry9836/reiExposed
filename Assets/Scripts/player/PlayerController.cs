using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float staminaAmount = 100.0f;
    public float staminaMaxAmount = 100.0f;

    public float staminaRegenSpeed = 1.0f;

    private void Start()
    {
        staminaAmount = staminaMaxAmount;
    }
    public void ChangeStamina(float amount)
    {
        staminaAmount += amount;
    }
    
    public float CheckStamina()
    {
        return staminaAmount;
    }

    private void Update()
    {
        if (staminaAmount < staminaMaxAmount)
        {
            staminaAmount += staminaRegenSpeed * Time.deltaTime;

            if (staminaAmount > staminaMaxAmount)
            {
                staminaAmount = staminaMaxAmount;
            }

        }
    }


}
