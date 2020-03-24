using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float maxHealth = 100.0f;
    public float health = 100.0f;
    public float staminaAmount = 100.0f;
    public float staminaMaxAmount = 100.0f;
    public float staminaRegenSpeed = 1.0f;

    private GameObject staminaUI;
    private GameObject HPui;
    private GameObject boss;
    private GameObject umberalla;

    private void Start()
    {
        staminaAmount = staminaMaxAmount;
        health = maxHealth;
        boss = GameObject.FindGameObjectWithTag("Boss");
        staminaUI = GameObject.Find("staminaUI");
        HPui = GameObject.Find("playersHP");
        umberalla = GameObject.Find("umbrella ella ella");
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
        uiupdate();

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
        if (other.gameObject.CompareTag("BossAttackSurface") && !umberalla.GetComponent<umbrella>().ISBLockjing)
        {
            health -= boss.GetComponent<BossController>().QueryDamage();
        }
        else if (other.gameObject.CompareTag("BossAttackSurface") && umberalla.GetComponent<umbrella>().ISBLockjing)
        {
            umberalla.GetComponent<umbrella>().cooldown = true;
            boss.GetComponent<BossController>().arm(BossController.ARMTYPE.ARM_ALL, false);
        }
    }

    void uiupdate()
    {
        staminaUI.GetComponent<Image>().fillAmount = staminaAmount / staminaMaxAmount;
        HPui.GetComponent<Image>().fillAmount = health / maxHealth;

    }

}
