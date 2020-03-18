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

    public bool animationOverride = false;
    public bool trackPlayer = true;
    public float turnSpeed = 0.1f;
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

    public void animationOverrideFunc(bool overrideSwitch)
    {
        animationOverride = overrideSwitch;
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

        if (trackPlayer)
        {
            agent.angularSpeed = 0.0f;
        }

        turnSpeed /= 1000.0f;

        if (arms.Count <= 0)
        {
            Debug.LogWarning("Boss Arm Count is 0");
        }

    }

    private void Update()
    {
        if (trackPlayer && !animationOverride)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            Quaternion endRot = Quaternion.LookRotation(dir, transform.up);

            //If we are we need to turn to face player stop agent so we don't tokyo drift
            float dotProd = Vector3.Dot(dir, transform.forward);
            float thresholdAngle = 0.95f;

            if (dotProd > thresholdAngle)
            {
                agent.isStopped = false;
            }
            else if (!agent.isStopped)
            {
                agent.isStopped = true;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, endRot, turnSpeed * Time.time);
        }
    }
}
