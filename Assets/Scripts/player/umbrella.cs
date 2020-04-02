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

    private PlayerController playercontrol;
    private GameObject cam;
    private GameObject boss;
    private GameObject umbeaalBone;
    private Animator animator;
    public GameObject VFX;
    private AudioSource audio;

    private bool latetest = false;
  

    void Start()
    {
        playercontrol = this.transform.parent.parent.GetComponent<PlayerController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        boss = GameObject.Find("Boss");
        umbeaalBone = GameObject.Find("rei_umbrella");
        animator = playercontrol.gameObject.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        latetest = false;

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
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
                    if (canfire == true)
                    {
                        firemode();
                    }
                }
                else
                {
                    cooldown = true;
                }
            }
            else
            {
                animator.SetBool("blocking", false);
            }
        }
        else
        {
            animator.SetBool("blocking", false);
            cooldowntimer += Time.deltaTime;
            if (cooldowntimer > cooldowntime)
            {
                cooldown = false;
                cooldowntimer = 0.0f;
            }
        }

    }

    void blocking()
    {
        animator.SetBool("blocking", true);
        animator.SetBool("alreadyBlocking", true);

    }

    void firemode()
    {
        latetest = true;
        VFX.GetComponent<VisualEffect>().SetFloat("timer", 1.0f);

        if (Input.GetAxis("Fire1") > 0.5f)
        {
            bang();
            animator.SetTrigger("shoot");
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

    public void whack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.parent.parent.position, 2.0f, enemy);

        if (hitColliders.Length != 0)
        {
            dodamage(hitColliders[0].ClosestPoint(transform.parent.parent.position), 25.0f);
        }
    }

    void dodamage(Vector3 pos, float attackingfor)
    {
        float damage = boss.GetComponent<BossController>().IBeanShot(attackingfor);

        GameObject text = Instantiate(damagedText, pos, Quaternion.identity);
        text.transform.GetChild(0).GetComponent<Text>().text = null;
        text.transform.GetChild(0).GetComponent<Text>().text = Mathf.RoundToInt(damage).ToString();
        text.transform.LookAt(cam.transform.position);
        text.transform.Rotate(new Vector3(0, 180, 0));
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
