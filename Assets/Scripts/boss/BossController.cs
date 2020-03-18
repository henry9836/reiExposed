using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public enum bossAttacks
    {
        CHARGE,
        MONKEYSLAM,
        BODYSLAM,
        SWIPE,
        FIREBALL
    };

    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public float neededAttackRange;


    public float health;
    public float maxHealth = 1000.0f;
    public List<float> attackTriggerRanges = new List<float>();
    public List<BoxCollider> arms = new List<BoxCollider>();


    public void armArms()
    {
        GetComponent<AudioSource>().Play();
        for (int i = 0; i < arms.Count; i++)
        {
            arms[i].enabled = true;
        }
    }

    public void disarmArms()
    {
        for (int i = 0; i < arms.Count; i++)
        {
            arms[i].enabled = false;
        }
    }

    private void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < arms.Count; i++)
        {
            arms[i].gameObject.AddComponent<hitSurfaceController>();
        }

        armArms();

    }

    public float IBeanShot(float damage)
    {
        float startHealth = health;
        float delt = 99999.9f;

        if (((health - damage) / maxHealth) > (this.gameObject.GetComponent<ghostEffect>().ghostpersent))
        {
            health -= damage;

            delt = damage;
            Debug.Log("full damage");
        }
        else if ((health / maxHealth) > (this.gameObject.GetComponent<ghostEffect>().ghostpersent))
        {
            health = maxHealth * (this.gameObject.GetComponent<ghostEffect>().ghostpersent);

            delt = startHealth - health;
            Debug.Log("not full damage");
        }
        else 
        {
            delt = 0.0f;
            Debug.Log("no damage");
        }

        return (delt);
    }
}
