using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float maxHealth = 100.0f;
    public float health = 100.0f;
    public float staminaAmount = 100.0f;
    public float staminaMaxAmount = 100.0f;
    public float staminaRegenSpeed = 1.0f;

    private GameObject boss;

    private void Start()
    {
        staminaAmount = staminaMaxAmount;
        health = maxHealth;
        boss = GameObject.FindGameObjectWithTag("Boss");
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

    private void OnTriggerEnter(Collider other)
    {
        //Damage From Boss
        if (other.gameObject.CompareTag("BossAttackSurface"))
        {
            health -= boss.GetComponent<BossController>().QueryDamage();
        }
    }

}
