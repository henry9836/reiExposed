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


    private PlayerController playercontrol;


    void Start()
    {
        playercontrol = this.transform.parent.parent.GetComponent<PlayerController>();
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
        if (Input.GetAxis("Fire1") > 0.5f)
        {
            bang();
        }
    }

    void bang()
    {
        //just aim better 
        Debug.Log("bang");
        cooldown = true;
    }
}
