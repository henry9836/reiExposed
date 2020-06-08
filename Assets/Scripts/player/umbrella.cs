using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class umbrella : MonoBehaviour
{
    public bool canfire = false;
    public float blockingStamina;
    public bool cooldown = false;
    public float cooldowntime = 2.0f;
    public float cooldowntimer = 0.0f;
    public LayerMask enemy;
    public LayerMask ball;
    public GameObject damagedText;
    public bool ISBLockjing = false;
    public List<AudioClip> swishSounds = new List<AudioClip>();
    public AudioClip umbrellaActivateSFX;
    public AudioClip umbrellaShoot;
    public BoxCollider umbrellaHitBox;
    public GameObject umbeaalBone;
    public GameObject boss;
    public AudioSource audio;

    private PlayerController playercontrol;
    private GameObject cam;
    private Animator animator;
    public GameObject VFX;

    private bool latetest = false;
  

    void Start()
    {
        playercontrol = GetComponent<PlayerController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        animator = playercontrol.gameObject.GetComponent<Animator>();
        if (!audio)
        {
            audio = GetComponent<AudioSource>();
        }
        umbrellaHitBox.enabled = false;
    }

    void Update()
    {
        latetest = false;

        if (Input.GetMouseButtonDown(0) && !animator.GetBool("Blocking"))
        {
            if (playercontrol.staminaAmount >= playercontrol.staminaToAttack) {
                playercontrol.ChangeStamina(-playercontrol.staminaToAttack);
                animator.SetTrigger("Attack");
            }
        }

        VFX.GetComponent<VisualEffect>().SetFloat("timer", 0.0f);


        if (cooldown == false)
        {
            if (Input.GetAxis("Fire2") > 0.5f)
            {
                if (playercontrol.staminaAmount > blockingStamina * Time.deltaTime)
                {
                    playercontrol.ChangeStamina(-blockingStamina * Time.deltaTime);
                    blocking();
                    //if (canfire)
                    //{
                        firemode();
                    //}
                }
                else
                {
                    cooldown = true;
                }
            }
            else
            {
                //animator.ResetTrigger("Block");
                animator.SetBool("Blocking", false);
            }
        }
        else
        {
            animator.SetBool("Blocking", false);
            cooldowntimer += Time.deltaTime;
            if (cooldowntimer > cooldowntime)
            {
                //animator.ResetTrigger("Block");
                cooldown = false;
                cooldowntimer = 0.0f;
            }
        }

    }

    void blocking()
    {
        if (!animator.GetBool("Blocking"))
        {
            animator.SetTrigger("Block");
            animator.SetBool("Blocking", true);
        }
    }

    void firemode()
    {
        latetest = true;
        VFX.GetComponent<VisualEffect>().SetFloat("timer", 1.0f);

        if (Input.GetAxis("Fire1") > 0.5f)
        {
            animator.SetTrigger("Shoot");
            bang();
        }
    }

    void bang()
    {
        RaycastHit hit;

        if (Physics.Raycast(umbeaalBone.transform.position, -umbeaalBone.transform.right, out hit, Mathf.Infinity, enemy))
        {
            dodamage(hit.point, 100.0f);

        }

        //just aim better 
        cooldown = true;
    }

    public void Hitbox(bool toggle)
    {
        umbrellaHitBox.enabled = toggle;
    }

    void dodamage(Vector3 pos, float attackingfor)
    {
        //COMMENTED OUT FOR MIGRATION NEEDS TO BE REBUILT FOR DIFFERENT ENEMYTYPES

        //float damage = boss.GetComponent<BossController>().IBeanShot(attackingfor);

        //GameObject text = Instantiate(damagedText, pos, Quaternion.identity);
        //text.transform.GetChild(0).GetComponent<Text>().text = null;
        //text.transform.GetChild(0).GetComponent<Text>().text = Mathf.RoundToInt(damage).ToString();
        //text.transform.LookAt(cam.transform.position);
        //text.transform.Rotate(new Vector3(0, 180, 0));
    }

    void LateUpdate()
    {
        if (latetest == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, ball))
            {
                
                umbeaalBone.transform.LookAt(hit.point);
                umbeaalBone.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
                Debug.DrawLine(hit.point, cam.transform.position);
            }
        }
    }
}
