﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;
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

    public enum ARMTYPE
    {
        ARM_LEFT_ARMS,
        ARM_RIGHT_ARMS,
        ARM_ARMS,
        ARM_BODY,
        ARM_LEFT_LEG,
        ARM_RIGHT_LEG,
        ARM_LEGS,
        ARM_ALL
    };

    enum UPDATE_MODE
    {
        DEFAULT,
        CHARGE_ATTACK
    };

    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public float neededAttackRange;
    [HideInInspector]
    public float lastUpdatedAttackDamage = 0.0f;

    public bool sleepOverride = false;
    public bool animationOverride = false;
    public bool trackPlayer = true;
    [Range(0.05f, 0.5f)]
    public float checkPlayerPositionInterval = 0.5f;
    public float turnSpeed = 0.1f;
    public float chargeSpeed = 30.0f;
    public float health;
    public float maxHealth = 1000.0f;
    [Range(0.0f, 1.0f)]
    public float angleThresholdBeforeMoving = 0.95f;
    public List<Vector2> attackTriggerRanges = new List<Vector2>();
    public List<BoxCollider> leftArms = new List<BoxCollider>();
    public List<BoxCollider> rightArms = new List<BoxCollider>();
    public List<BoxCollider> leftLegs = new List<BoxCollider>();
    public List<BoxCollider> rightLegs = new List<BoxCollider>();
    public List<BoxCollider> otherBody = new List<BoxCollider>();
    public List<Transform> fireBallCannonLocations = new List<Transform>();

    //Sounds
    public List<AudioClip> hurtSounds = new List<AudioClip>();

    public UnityEvent onDeath;
   

    private bool onlyApplyDamageOnce = true;
    private bool alreadyAppliedDamage = false;
    private bool deathonce = true;
    private UPDATE_MODE updateMode = UPDATE_MODE.DEFAULT;
    private Vector3 lastKnownPlayerPosition = Vector3.zero;
    private float playerCheckTimer = 0.0f;
    private AudioSource audio;

    public void sleepOverrideFunc(bool mode)
    {
        sleepOverride = mode;

        if (!mode)
        {
            GetComponent<Animator>().SetBool("Idle", false);
        }

    }
    public Vector3 predictPlayerPosition(float projectileSpeed, GameObject projectile)
    {
        //Positions
        Vector3 result = Vector3.zero;
        Vector3 currentPosition = player.transform.position;

        //Get Direction
        Vector3 directionOfMovement = (currentPosition - lastKnownPlayerPosition).normalized;

        //Get velocity
        float time = checkPlayerPositionInterval + playerCheckTimer;
        float distance = Vector3.Distance(currentPosition, lastKnownPlayerPosition);
        float velocity = distance / time;
        Vector3 targetVelocity = directionOfMovement * velocity;


        //velocity *= velocity;

        //Get Time to hit target
        float timeToHit = distance / projectileSpeed;

        //Get Predicted Position
        result = currentPosition + ((directionOfMovement * velocity)*timeToHit)*65.0f;


        Debug.DrawRay(projectile.transform.position, result, Color.red, 2.0f);

        return result;
    }

    public void arm(ARMTYPE type, bool arm, float attackDamage, bool _onlyApplyDamageOnce)
    {
        lastUpdatedAttackDamage = attackDamage;
        onlyApplyDamageOnce = _onlyApplyDamageOnce;
        alreadyAppliedDamage = false;
        switch (type)
        {
            case ARMTYPE.ARM_LEFT_ARMS:
                {
                    for (int i = 0; i < leftArms.Count; i++)
                    {
                        leftArms[i].enabled = arm;
                    }
                    break;
                }
            case ARMTYPE.ARM_RIGHT_ARMS:
                {
                    for (int i = 0; i < rightArms.Count; i++)
                    {
                        rightArms[i].enabled = arm;
                    }
                    break;
                }
            case ARMTYPE.ARM_ARMS:
                {
                    for (int i = 0; i < leftArms.Count; i++)
                    {
                        leftArms[i].enabled = arm;
                    }
                    for (int i = 0; i < rightArms.Count; i++)
                    {
                        rightArms[i].enabled = arm;
                    }
                    break;
                }
            case ARMTYPE.ARM_BODY:
                {
                    for (int i = 0; i < otherBody.Count; i++)
                    {
                        otherBody[i].enabled = arm;
                    }
                    break;
                }
            case ARMTYPE.ARM_LEFT_LEG:
                {
                    for (int i = 0; i < leftLegs.Count; i++)
                    {
                        leftLegs[i].enabled = arm;
                    }
                    break;
                }
            case ARMTYPE.ARM_RIGHT_LEG:
                {
                    for (int i = 0; i < rightLegs.Count; i++)
                    {
                        rightLegs[i].enabled = arm;
                    }
                    break;
                }
            case ARMTYPE.ARM_LEGS:
                {
                    for (int i = 0; i < leftLegs.Count; i++)
                    {
                        leftLegs[i].enabled = arm;
                    }
                    for (int i = 0; i < rightLegs.Count; i++)
                    {
                        rightLegs[i].enabled = arm;
                    }
                    break;
                }
            case ARMTYPE.ARM_ALL:
                {
                    for (int i = 0; i < leftArms.Count; i++)
                    {
                        leftArms[i].enabled = arm;
                    }
                    for (int i = 0; i < rightArms.Count; i++)
                    {
                        rightArms[i].enabled = arm;
                    }
                    for (int i = 0; i < otherBody.Count; i++)
                    {
                        otherBody[i].enabled = arm;
                    }
                    for (int i = 0; i < leftLegs.Count; i++)
                    {
                        leftLegs[i].enabled = arm;
                    }
                    for (int i = 0; i < rightLegs.Count; i++)
                    {
                        rightLegs[i].enabled = arm;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void arm(ARMTYPE _type, bool _arm)
    {
        arm(_type, _arm, 0.0f, true);
    }

    public float QueryDamage()
    {
        float result = lastUpdatedAttackDamage;

        if (onlyApplyDamageOnce && alreadyAppliedDamage)
        {
            lastUpdatedAttackDamage = 0.0f;
        }

        alreadyAppliedDamage = true;
        return result;
    }

    public void animationOverrideFunc(bool overrideSwitch)
    {
        animationOverride = overrideSwitch;

        if (animationOverride)
        {
            agent.ResetPath();
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

    }

    public void charge()
    {
        updateMode = UPDATE_MODE.CHARGE_ATTACK;
    }

    private void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        lastKnownPlayerPosition = player.transform.position;

        if (trackPlayer)
        {
            agent.angularSpeed = 0.0f;
        }
        GetComponent<Animator>().SetBool("Idle", true);
        audio = GetComponent<AudioSource>();
    }

    public bool isBossLookingAtPlayer(float thresholdAngle)
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;

        //If we are we need to turn to face player stop agent so we don't tokyo drift
        float dotProd = Vector3.Dot(dir, transform.forward);

        return dotProd > thresholdAngle;
    }

    private void Update()
    {
        if (!sleepOverride)
        {

            playerCheckTimer += Time.deltaTime;

            if (playerCheckTimer > checkPlayerPositionInterval)
            {
                lastKnownPlayerPosition = player.transform.position;
                playerCheckTimer = 0.0f;
            }

            if (deathonce)
            {
                if (health <= 0.0f)
                {
                    deathonce = false;
                    death();
                    return;
                }


                if (updateMode == UPDATE_MODE.DEFAULT)
                {

                    if (trackPlayer && !animationOverride)
                    {
                        Vector3 dir = (player.transform.position - transform.position).normalized;
                        Quaternion endRot = Quaternion.LookRotation(dir, transform.up);


                        if (isBossLookingAtPlayer(angleThresholdBeforeMoving))
                        {
                            agent.isStopped = false;
                        }
                        else if (!agent.isStopped)
                        {
                            agent.isStopped = true;
                        }

                        //transform.rotation = Quaternion.Lerp(transform.rotation, endRot, 0.005f);

                        transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, turnSpeed * Time.deltaTime);

                    }


                }
                else if (updateMode == UPDATE_MODE.CHARGE_ATTACK)
                {
                    transform.Translate(transform.forward * Time.deltaTime * chargeSpeed, Space.World);
                }
                else
                {
                    Debug.LogWarning($"Cannot run boss update as [{updateMode}] has no update behaviour");
                }
            }
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.isStopped = false;
        }
    }

    public float IBeanShot(float damage)
    {
        float startHealth = health;
        float delt = 99999.9f;

        if (((health - damage) / maxHealth) > (this.gameObject.GetComponent<ghostEffect>().ghostpersent))
        {
            health -= damage;

            delt = damage;
        }
        else if ((health / maxHealth) > (this.gameObject.GetComponent<ghostEffect>().ghostpersent))
        {
            health = maxHealth * (this.gameObject.GetComponent<ghostEffect>().ghostpersent);

            delt = startHealth - health;
            this.gameObject.GetComponent<ghostEffect>().shakeIcon();
        }
        else
        {
            delt = 0.0f;
            this.gameObject.GetComponent<ghostEffect>().shakeIcon();

        }

        audio.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Count)]);

        return (delt);
    }

    void death()
    {
        onDeath.Invoke();
        GetComponent<Animator>().SetTrigger("Dead");
        agent.isStopped = true;
        agent.ResetPath();
        agent.isStopped = false;
        this.gameObject.GetComponent<ghostEffect>().UIHP.GetComponent<Image>().fillAmount = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (updateMode == UPDATE_MODE.CHARGE_ATTACK)
        {
            //If not ground
            if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                GetComponent<Animator>().SetBool("Charging", false);
                animationOverrideFunc(false);
                updateMode = UPDATE_MODE.DEFAULT;
            }
        }
    }

}