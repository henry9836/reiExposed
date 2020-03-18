using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public float neededAttackRange;
    [HideInInspector]
    public float lastUpdatedAttackDamage = 0.0f;

    public bool animationOverride = false;
    public bool trackPlayer = true;
    public float turnSpeed = 0.1f;
    public float health;
    public float maxHealth = 1000.0f;
    public List<float> attackTriggerRanges = new List<float>();
    public List<BoxCollider> leftArms = new List<BoxCollider>();
    public List<BoxCollider> rightArms = new List<BoxCollider>();
    public List<BoxCollider> leftLegs = new List<BoxCollider>();
    public List<BoxCollider> rightLegs = new List<BoxCollider>();
    public List<BoxCollider> otherBody = new List<BoxCollider>();

    private bool onlyApplyDamageOnce = true;
    private bool deathonce = true;

    public void arm(ARMTYPE type, bool arm, float attackDamage, bool _onlyApplyDamageOnce)
    {
        lastUpdatedAttackDamage = attackDamage;
        onlyApplyDamageOnce = _onlyApplyDamageOnce;
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


    public void animationOverrideFunc(bool overrideSwitch)
    {
        animationOverride = overrideSwitch;
    }

    private void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (trackPlayer)
        {
            agent.angularSpeed = 0.0f;
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

            //transform.rotation = Quaternion.Lerp(transform.rotation, endRot, 0.005f);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, turnSpeed * Time.deltaTime);

        }

        if (deathonce == true)
        {
            if (health <= 0.0f)
            {
                deathonce = false;
                death();
            }
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

    void death()
    {
        this.gameObject.GetComponent<ghostEffect>().UIHP.GetComponent<Image>().fillAmount = 0.0f;
        Destroy(this.gameObject);


    }
}
