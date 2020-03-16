using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class umbrella : MonoBehaviour
{
    public bool canfire = true;
    public float blockingStamina;
    public bool cooldown = false;
    public float cooldowntime = 2.0f;
    public float cooldowntimer = 0.0f;
    public LayerMask enemy;
    public LayerMask ball;


    private PlayerController playercontrol;
    private GameObject cam;


    void Start()
    {
        playercontrol = this.transform.parent.parent.GetComponent<PlayerController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
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
            Debug.Log(hit.collider.gameObject.layer);
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
            //damage
        }

        //just aim better 
        cooldown = true;
    }
}
