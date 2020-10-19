using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
    public float staminaToHeavyAttack = 15.0f;
    //[HideInInspector]
    public bool staminaBlock = false;

    [Header("Combat")]
    public float umbreallaDmg = 20.0f;
    public float umbreallaHeavyDmg = 50.0f;
    public float knockDownThreshold = 15.0f;

    [Header("Death")]
    public bool dead = false;

    [Header("Damage")]
    public Color maxcolor;
    public Color minColor;
    public Image damaged;

    [Header("VFX")]
    public GameObject bossHitVFX;
    public GameObject blockVFX;
    public GameObject hitVFX;

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


    public GameObject bossDeathCam;
    public GameObject mythDeathCam;
    public GameObject fogThing;

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
            gameObject.GetComponent<Animator>().SetBool("DeathOverride", true);
            dead = true;
            audio.PlayOneShot(deathSound);
            StartCoroutine(death());

        }
    }

    private void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            Debug.Log(SaveSystemController.checkSaveValid());
            Debug.Log(SaveSystemController.calcCurrentHash());
        }

#endif

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

                AIAttackContainer.EFFECTTYPES effect = AIAttackContainer.EFFECTTYPES.NONE;

                //Apply Damage
                if (otherObject.transform.root.GetComponent<AIObject>() != null)
                {
                    //Stun based on type
                    effect = otherObject.transform.root.GetComponent<AIObject>().QueryDamageEffect();
                    health -= otherObject.transform.root.GetComponent<AIObject>().QueryDamage();
                }
                else if (otherObject.GetComponent<GenericHitboxController>() != null)
                {
                    Collider col = GetComponent<Collider>();
                    Debug.DrawLine(other.ClosestPointOnBounds(col.transform.position), col.transform.position, Color.magenta, 10.0f, false);
                    float dmg = otherObject.GetComponent<GenericHitboxController>().Damage();
                    //Stun based on type
                    effect = otherObject.GetComponent<GenericHitboxController>().effect;
                    health -= dmg;
                    Debug.Log($"Took Damage {dmg}");
                }
                else
                {
                    Debug.LogWarning($"Unknown Component Damage {otherObject.name}");
                }
                audio.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Count)]);

                //Stun Animation
                switch (effect)
                {
                    case AIAttackContainer.EFFECTTYPES.STUN:
                        {
                            animator.SetTrigger("Stun");
                            break;
                        }
                    case AIAttackContainer.EFFECTTYPES.KNOCKBACK:
                        {
                            animator.SetTrigger("KnockBack");
                            break;
                        }
                    case AIAttackContainer.EFFECTTYPES.KNOCKDOWN:
                        {
                            animator.SetTrigger("KnockDown");
                            break;
                        }
                    default:
                        break;
                }

                //Boss VFX Hit
                if (otherObject.transform.root.tag == "Boss")
                {
                    //Spawn VFX
                    Instantiate(bossHitVFX, GetComponent<Collider>().ClosestPointOnBounds(otherObject.transform.position), Quaternion.identity);
                }
                else
                {
                    //Spawn VFX
                    Instantiate(hitVFX, GetComponent<Collider>().ClosestPointOnBounds(otherObject.transform.position), Quaternion.identity);
                }
            }
            //If we are blocking
            else if (other.gameObject.CompareTag("EnemyAttackSurface") && umbrella.ISBLockjing)
            {
                Debug.Log("I was hit and but blocked");
                umbrella.cooldown = true;

                //Disable hitboxes
                boss.GetComponent<AIObject>().body.updateHitBox(AIBody.BodyParts.ALL, false);
                Instantiate(blockVFX, other.transform.position, Quaternion.identity);
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

        if (boss.GetComponent<Animator>().GetBool("Sleeping"))
        {
            bossDeathCam.SetActive(true);
        }
        else
        {
            mythDeathCam.SetActive(true);
        }

        int half = Mathf.RoundToInt(SaveSystemController.getFloatValue("MythTraces") * 0.5f);
        SaveSystemController.updateValue("MythTraces", half);
        SaveSystemController.saveDataToDisk();

        deathUI[5].GetComponent<Text>().text = "You panicked and dropped " + half.ToString() + "¥.\n\nYou blacked out!";
        deathUI[5].SetActive(true);
        fogThing.GetComponent<FogFollow>().followThePlayer = false;


        for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime)
        {
            deathUI[0].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, Mathf.Lerp(0.0f, 1.0f, i));

            yield return null;
        }
        deathUI[0].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        deathUI[0].SetActive(false);


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
