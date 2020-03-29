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

    private List<GameObject> deathUI = new List<GameObject>() { };

    public bool dead = false;

    private void Start()
    {
        staminaAmount = staminaMaxAmount;
        health = maxHealth;
        boss = GameObject.FindGameObjectWithTag("Boss");
        staminaUI = GameObject.Find("staminaUI");
        HPui = GameObject.Find("playersHP");
        umberalla = GameObject.Find("umbrella ella ella");
        GameObject temp = GameObject.Find("deathUI");

        for (int i = 0; i < temp.transform.childCount; i++)
        {
            deathUI.Add(temp.transform.GetChild(i).gameObject);
        }
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
        if (dead == false)
        {
            //Damage From Boss
            if (other.gameObject.CompareTag("BossAttackSurface") && !umberalla.GetComponent<umbrella>().ISBLockjing)
            {

                Debug.Log($"Boss Hit me with {other.gameObject.name}");

                health -= boss.GetComponent<BossController>().QueryDamage();
            }
            else if (other.gameObject.CompareTag("BossAttackSurface") && umberalla.GetComponent<umbrella>().ISBLockjing)
            {
                Debug.Log($"Boss Hit me with {other.gameObject.name} but I blocked it");

                umberalla.GetComponent<umbrella>().cooldown = true;
                boss.GetComponent<BossController>().arm(BossController.ARMTYPE.ARM_ALL, false);
            }

            if (health <= 0.0f)
            {
                this.gameObject.GetComponent<Animator>().SetTrigger("deathT");
                dead = true;
                StartCoroutine(death());
            }
        }
    }

    void uiupdate()
    {
        staminaUI.GetComponent<Image>().fillAmount = staminaAmount / staminaMaxAmount;
        HPui.GetComponent<Image>().fillAmount = health / maxHealth;

    }


    public IEnumerator death()
    {
        deathUI[0].SetActive(true);

        for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime * 0.4f)
        {
            deathUI[0].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, Mathf.Lerp(0.0f, 1.0f, i));

            yield return null;
        }
        deathUI[0].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        yield return new WaitForSeconds(0.5f);

        deathUI[1].SetActive(true);

        yield return new WaitForSeconds(0.3f);
        deathUI[1].SetActive(false);
        yield return new WaitForSeconds(1.0f);


        deathUI[2].SetActive(true);
        deathUI[3].SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        yield return null;
    }

}
