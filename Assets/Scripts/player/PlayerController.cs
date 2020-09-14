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
    public float staminaToAttack = 5.0f;
    [HideInInspector]
    public bool staminaBlock = false;

    [Header("Combat")]
    public float umbreallaDmg = 5.0f;

    [Header("Death")]
    public bool dead = false;

    [Header("Damage")]
    public Color maxcolor;
    public Color minColor;
    public Image damaged;
    public GameObject bossHitVFX;

    //Sounds
    public List<AudioClip> hurtSounds = new List<AudioClip>();
    public AudioClip deathSound;

    private GameObject staminaUI;
    private GameObject HPui;
    private GameObject boss;
    private List<GameObject> deathUI = new List<GameObject>();
    private AudioSource audio;
    private umbrella umbrella;
    private bool UIon = false;
    private Animator animator;


    private void Start()
    {
        staminaAmount = staminaMaxAmount;
        health = maxHealth;
        boss = GameObject.FindGameObjectWithTag("Boss");
        staminaUI = GameObject.Find("staminaUI");
        HPui = GameObject.Find("playersHP");
        GameObject temp = GameObject.Find("IntroAndDeathUI");

        for (int i = 0; i < temp.transform.childCount; i++)
        {
            deathUI.Add(temp.transform.GetChild(i).gameObject);
        }

        audio = GetComponent<AudioSource>();
        umbrella = GetComponent<umbrella>();
        animator = GetComponent<Animator>();
    }
    public void ChangeStamina(float amount)
    {
        staminaAmount += amount;
    }
    
    public float CheckStamina()
    {
        return staminaAmount;
    }

    public void CheckDeath()
    {
        if (health <= 0.0f)
        {
            gameObject.GetComponent<Animator>().SetTrigger("Death");
            dead = true;
            audio.PlayOneShot(deathSound);
            StartCoroutine(death());

        }
    }

    private void Update()
    {
        uiupdate();

        if (!staminaBlock) {
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

    //Changes health value of player
    public void EffectHeatlh(float amount)
    {
        health += amount;
        if (amount < 0)
        {
            audio.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Count)]);
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    void uiupdate()
    {
        staminaUI.GetComponent<Image>().fillAmount = staminaAmount / staminaMaxAmount;
        HPui.GetComponent<Image>().fillAmount = health / maxHealth;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (dead == false)
        {
            GameObject otherObject = other.gameObject;
            //Damage From Enemy and we are not blocking
            if (otherObject.CompareTag("EnemyAttackSurface") && !umbrella.ISBLockjing)
            {
                Debug.Log("I was hit and taking damage");

                if (otherObject.transform.root.GetComponent<AIObject>() != null)
                {
                    health -= otherObject.transform.root.GetComponent<AIObject>().QueryDamage();
                }
                else if (otherObject.GetComponent<GenericHitboxController>() != null)
                {
                    Collider col = GetComponent<Collider>();
                    Debug.DrawLine(other.ClosestPointOnBounds(col.transform.position), col.transform.position, Color.magenta, 10.0f, false);
                    float dmg = otherObject.GetComponent<GenericHitboxController>().Damage();
                    health -= dmg;
                    Debug.Log($"Took Damage {dmg}");
                }
                else
                {
                    Debug.LogWarning($"Unknown Component Damage {otherObject.name}");
                }
                audio.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Count)]);

                //Boss VFX Hit
                if (otherObject.transform.root.tag == "Boss")
                {
                    //Spawn VFX
                    Instantiate(bossHitVFX, GetComponent<Collider>().ClosestPointOnBounds(otherObject.transform.position), Quaternion.identity);
                }

                //Stun
                animator.SetTrigger("KnockDown");
            }
            else if (other.gameObject.CompareTag("EnemyAttackSurface") && umbrella.ISBLockjing)
            {
                Debug.Log("I was hit and but blocked");
                umbrella.cooldown = true;

                //Disable hitboxes
                boss.GetComponent<AIObject>().body.updateHitBox(AIBody.BodyParts.ALL, false);

                //Stun
                animator.SetTrigger("KnockDown");
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
                gameObject.GetComponent<Animator>().SetTrigger("Death");
                dead = true;
                audio.PlayOneShot(deathSound);
                StartCoroutine(death());
                
            }
        }
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

        if (!dead && health <= 40.0f)
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
