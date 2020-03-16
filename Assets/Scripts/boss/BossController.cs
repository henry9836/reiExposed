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
    }
}
