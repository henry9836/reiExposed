using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class umbrella : MonoBehaviour
{
    public bool canfire = true;
    public float blockingStamina;
    public bool cooldown = false;
    public float cooldowntime = 2.0f;
    public float cooldowntimer = 0.0f;
    public LayerMask enemy;
    public LayerMask ball;
    public GameObject damagedText;
    public bool ISBLockjing = true;

    private PlayerController playercontrol;
    private GameObject cam;
    private GameObject boss;
    private GameObject umbeaalBone;
    private Animator animator;


    void Start()
    {
        playercontrol = this.transform.parent.parent.GetComponent<PlayerController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        boss = GameObject.Find("Boss");
        umbeaalBone = GameObject.Find("rei_umbrella");
        animator = playercontrol.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        ISBLockjing = false;

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        if (cooldown == false)
        {
            if (Input.GetAxis("Fire2") > 0.5f)
            {
                if (playercontrol.staminaAmount > blockingStamina)
                {
                    playercontrol.ChangeStamina(-blockingStamina);

                    blocking();
                    if (canfire == true)
                    {
                        firemode();
                    }
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
        ISBLockjing = true;
        animator.SetBool("blocking", true);
        animator.SetBool("alreadyBlocking", true);

    }

    void firemode()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, ball))
        {
            this.gameObject.transform.LookAt(hit.point);
            Debug.DrawLine(hit.point, cam.transform.position);
        }




        if (Input.GetAxis("Fire1") > 0.5f)
        {
            bang();
        }
    }

    void bang()
    {
        Debug.Log("bang");
        animator.SetTrigger("shoot");
        RaycastHit hit;

        if (Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out hit, Mathf.Infinity, enemy))
        {
            dodamage(hit.point, 100.0f);

        }

        //just aim better 
        cooldown = true;
    }

    public void whack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.parent.parent.position, 2.0f, enemy);

        Debug.Log("wak");
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
}
