using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hax : MonoBehaviour
{

    PlayerController pc;
    AIObject boss;

    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<AIObject>();
    }

    private void FixedUpdate()
    {
        //Health Hax
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (pc.maxHealth > 100)
            {
                pc.maxHealth = 100.0f;
                pc.health = 100.0f;
            }
            else
            {
                pc.maxHealth = 99999.0f;
                pc.health = 99999.0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            boss.health -= 250.0f;
        }

        //Keep hax active
        if (pc.maxHealth > 100)
        {
            pc.health = 99999.0f;
        }


    }
}
