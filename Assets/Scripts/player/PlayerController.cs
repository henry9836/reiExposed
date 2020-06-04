using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("Health")]
    public float maxHealth = 100.0f;
    public float health = 100.0f;

    [Header("Stamina")]
    public float staminaAmount = 100.0f;
    public float staminaMaxAmount = 100.0f;
    public float staminaRegenSpeed = 1.0f;

    [Header("Death")]
    public bool dead = false;

    [Header("Damage")]
    public Color maxcolor;
    public Color minColor;
    public Image damaged;
    //Sounds
    public List<AudioClip> hurtSounds = new List<AudioClip>();
    public AudioClip deathSound;

    private GameObject staminaUI;
    private GameObject HPui;
    private GameObject boss;
    [HideInInspector]
    public GameObject umberalla;
    private List<GameObject> deathUI = new List<GameObject>();
    private AudioSource audio;
    private bool UIon = false;



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

        audio = GetComponent<AudioSource>();
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
            //Damage From Enemy and we are not blocking
            if (other.gameObject.CompareTag("EnemyAttackSurface") && !umberalla.GetComponent<umbrella>().ISBLockjing)
            {
                Debug.Log("I was hit and taking damage");
                health -= other.gameObject.GetComponent<DamageQuery>().QueryDamage();
                audio.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Count)]);
            }

            //Damage From Boss
            else if (other.gameObject.CompareTag("BossAttackSurface") && !umberalla.GetComponent<umbrella>().ISBLockjing)
            {
                Debug.Log("I was hit and taking damage");
                health -= boss.GetComponent<BossController>().QueryDamage();
                audio.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Count)]);
            }
            else if (other.gameObject.CompareTag("BossAttackSurface") && umberalla.GetComponent<umbrella>().ISBLockjing)
            {
                Debug.Log("I was hit and but blocked");
                umberalla.GetComponent<umbrella>().cooldown = true;
                boss.GetComponent<BossController>().arm(BossController.ARMTYPE.ARM_ALL, false);
            }
            else
            {
                Debug.Log("I was hit and but ignoring");
            }


            if (health <= 40.0f)
            {
                if (UIon == false)
                {
                    UIon = true;
                    StartCoroutine(UIflash(true));
                }
            }

            if (health <= 0.0f)
            {
                this.gameObject.GetComponent<Animator>().SetTrigger("deathT");
                dead = true;
                audio.PlayOneShot(deathSound);
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

        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * 0.4f)
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


    public IEnumerator UIflash(bool first)
    {
        if (first == true)
        {
            for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime)
            {
                damaged.color = Color.Lerp(new Color(0.0f, 0.0f, 0.0f, 0.0f), maxcolor, i);
                yield return null;
            }
            for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime)
            {
                damaged.color = Color.Lerp(maxcolor, minColor, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime)
            {
                damaged.color = Color.Lerp(minColor, maxcolor, i);
                yield return null;
            }
            for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime)
            {
                damaged.color = Color.Lerp(maxcolor, minColor, i);
                yield return null;
            }

        }

        if (!dead)
        {
            StartCoroutine(UIflash(false));
        }
        else
        {
            StartCoroutine(flashoff());
        }


        yield return null;
    }

    public IEnumerator flashoff()
    {

        Color start = damaged.color;
        for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime)
        {
            damaged.color = Color.Lerp(start, new Color(0.0f, 0.0f, 0.0f, 0.0f), i);
            yield return null;
        }
        damaged.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        yield return null;
    }

}
