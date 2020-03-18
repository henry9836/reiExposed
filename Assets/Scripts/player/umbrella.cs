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

    private PlayerController playercontrol;
    private GameObject cam;
    private GameObject boss;

    void Start()
    {
        playercontrol = this.transform.parent.parent.GetComponent<PlayerController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        boss = GameObject.Find("Boss");
    }

    void Update()
    {
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
        }
        else
        {
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
        //if (would take damage)
        //{ 
        //dont 
        //cooldown = true;
        //}
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

        RaycastHit hit;

        if (Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out hit, Mathf.Infinity, enemy))
        {
            float damage = boss.GetComponent<BossController>().IBeanShot(100.0f);
            GameObject text = Instantiate(damagedText, hit.point, Quaternion.identity);
            text.transform.GetChild(0).GetComponent<Text>().text = null;
            text.transform.GetChild(0).GetComponent<Text>().text = Mathf.RoundToInt(damage).ToString();
            text.transform.LookAt(this.gameObject.transform.position);
            text.transform.Rotate(new Vector3(0, 180, 0));
        }

        //just aim better 
        cooldown = true;
    }
}
