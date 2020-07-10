using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UI;

public class ReprisialOfFlameController : MonoBehaviour
{

    public enum ATTACKSURFACES
    {
        ALL,
        ARMS,
        LEGS,
        OTHER,
        LEFTARM,
        RIGHTARM,
        LEFTLEG,
        RIGHTLEG,
    }

    public class attack
    {
        public string name;
        public float damage;
        public bool damageOnlyOnce;
        public bool attackIsBool;
        public Vector2 range;

        public attack(string _name, Vector2 _range, float _dmg, bool _isB)
        {
            name = _name;
            range = _range;
            damage = _dmg;
            attackIsBool = _isB;
        }
    }

    public bool goTime = false;

    //Defined Values
    [Header("General Settings")]
    public bool sleepOveride;
    public float health = 100.0f;
    [Range(0.0f, 1.0f)]
    public float thresholdSightAngle = 0.5f;
    public float stuckTimerThreshold = 5.0f;
    public float stuckVeloThreshold = 1.0f;
    public float regenSpeed = 0.0f;
    public Transform dashCheck;
    public LayerMask sightObstacles;
    public LayerMask groundLayers;
    public LayerMask dashObstacles;

    [Header("Movement Settings")]
    public float movementSpeed = 3.5f;
    public float movementSpeedAgro = 5f;
    public float turnSpeed = 120.0f;
    public float turnSpeedAgro = 250.0f;
    public Vector2 stayAfterArrivalTimeRange = new Vector2(0.0f, 7.0f);
    public float arriveDistanceThreshold = 1.0f; 
    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onHurt;
    public UnityEvent onStart;
    [Range(0.05f, 1.0f)]
    public float thresholdBeforeUnlock = 0.2f;
    public GameObject fireHead;

    [Header("Body Parts")]
    public Transform eyes;
    public List<BoxCollider> leftArms = new List<BoxCollider>();
    public List<BoxCollider> rightArms = new List<BoxCollider>();
    public List<BoxCollider> leftLegs = new List<BoxCollider>();
    public List<BoxCollider> rightLegs = new List<BoxCollider>();
    public List<BoxCollider> otherBody = new List<BoxCollider>();

    [Header("Moveset")]
    public List<string> attacks = new List<string>();
    public List<Vector2> attackRanges = new List<Vector2>();
    public List<float> attackDmg = new List<float>();
    public List<bool> attackIsBool = new List<bool>();

    [Header("UI Settings")]
    public Image lockedUI;
    public Image unlockedUI;
    public Image healthUI;
    public Image ghostUI;

    [Header("VFX Settings")]
    public Animator vfxBodyAnimatior;
    public VFXController vfxCtrl;

    [Header("Debug")]
    public bool debugMode;
    public string currentMode = "PENDING";

    //Hidden Values
    [HideInInspector]
    public float startHealth;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public Vector3 lastKnownPlayerPosition;
    [HideInInspector]
    public Transform playerTargetNode;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public attack currentAttack;
    [HideInInspector]
    public Vector3 target;



    private Animator animator;
    private NavMeshPath path;
    private PlayerController pc;

    //PLAYER DAMAGE QUERY
    public float QueryDamage()
    {
        if (currentAttack != null)
        {
            float dmg = currentAttack.damage;
            if (currentAttack.damageOnlyOnce)
            {
                UpdateAttackSurface(ATTACKSURFACES.ALL, false, false);
                clearAttack();
            }

            return dmg;
        }
        else
        {
            Debug.LogWarning("QUERY REQUEST RECEIVED BUT THERE IS NO CURRENT ATTACK ASSIGNED" + gameObject.name);
            return 0.0f;
        }
    }


    //ATTACK
    public void UpdateAttackSurface(ATTACKSURFACES surface, bool arm, bool _damageOnlyOnce)
    {
        switch (surface)
        {
            case ATTACKSURFACES.ALL:
                {
                    for (int i = 0; i < leftArms.Count; i++)
                    {
                        leftArms[i].enabled = arm;
                    }
                    for (int i = 0; i < rightArms.Count; i++)
                    {
                        rightArms[i].enabled = arm;
                    }
                    for (int i = 0; i < leftLegs.Count; i++)
                    {
                        leftLegs[i].enabled = arm;
                    }
                    for (int i = 0; i < rightLegs.Count; i++)
                    {
                        rightLegs[i].enabled = arm;
                    }
                    for (int i = 0; i < otherBody.Count; i++)
                    {
                        otherBody[i].enabled = arm;
                    }
                    break;
                }
            case ATTACKSURFACES.ARMS:
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
            case ATTACKSURFACES.LEGS:
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
            case ATTACKSURFACES.OTHER:
                {
                    for (int i = 0; i < otherBody.Count; i++)
                    {
                        otherBody[i].enabled = arm;
                    }
                    break;
                }
            case ATTACKSURFACES.LEFTARM:
                {
                    for (int i = 0; i < leftArms.Count; i++)
                    {
                        leftArms[i].enabled = arm;
                    }
                    break;
                }
            case ATTACKSURFACES.RIGHTARM:
                {
                    for (int i = 0; i < rightArms.Count; i++)
                    {
                        rightArms[i].enabled = arm;
                    }
                    break;
                }
            case ATTACKSURFACES.LEFTLEG:
                {
                    for (int i = 0; i < leftLegs.Count; i++)
                    {
                        leftLegs[i].enabled = arm;
                    }
                    break;
                }
            case ATTACKSURFACES.RIGHTLEG:
                {
                    for (int i = 0; i < rightLegs.Count; i++)
                    {
                        rightLegs[i].enabled = arm;
                    }
                    break;
                }
            default:
                {
                    Debug.LogError($"No valid attacksurface group logic found! {gameObject.name}");
                    break;
                }
        }
        if (currentAttack != null)
        {
            currentAttack.damageOnlyOnce = _damageOnlyOnce;
        }
    }

    //Attack Picking
    public attack getAttack()
    {
        if (currentAttack == null)
        {
            return pickAttack();
        }
        return currentAttack;
    }

    //Pick an attack
    public attack pickAttack()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, lastKnownPlayerPosition);
        float shortestDistance = Mathf.Infinity;
        float testingDis = 0.0f;
        attack tmp;
        List<attack> Foundattacks = new List<attack>();

        //For all attacks
        for (int i = 0; i < attacks.Count; i++)
        {
            //Select next attack
            tmp = new attack(attacks[i], attackRanges[i], attackDmg[i], attackIsBool[i]);
            //How close do we need to get to start attacking?
            testingDis = Mathf.Abs(distanceFromPlayer - tmp.range.x);
            //If this the shortest distance we have found?
            if (testingDis < shortestDistance)
            {
                Foundattacks.Clear();
                Foundattacks.Add(tmp);
                shortestDistance = testingDis;
                currentAttack = tmp;
            }
            else if (testingDis == shortestDistance)
            {
                Foundattacks.Add(tmp);
            }
        }

        if (Foundattacks.Count > 1)
        {
            currentAttack = Foundattacks[Random.Range(0, Foundattacks.Count)];
        }

        Debug.Log("Picked: " + currentAttack.name);
        return currentAttack;
    }
    public bool hasAttack()
    {
        return (currentAttack != null);
    }
    public void clearAttack()
    {
        Debug.Log("cleared attack");
        currentAttack = null;
    }

    //AI Movement
    public void stopMovement()
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.isStopped = false;
    }
    
    public bool GoToTargetPos(GameObject _target)
    {
        return GoToTargetPos(_target.transform.position);
    }

    public bool GoToTargetPos(Vector3 _target)
    {
        agent.isStopped = true;
        path = new NavMeshPath();
        bool tmp = agent.CalculatePath(_target, path);
        if (!tmp)
        {
            Debug.Log("Invalid Path!");
            return false;
        }
        else if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Invalid Path!");
            return false;
        }

        agent.SetPath(path);
        target = _target;
        agent.isStopped = false;
        return true;
    }

    //Health
    public void ChangeHealth(float amount)
    {
        health += amount;
        //Clamp
        health = Mathf.Clamp(health, -1.0f, startHealth);
    }

    //Sight
    public bool isLookingAtPlayer() { return isLookingAtPlayer(thresholdSightAngle); }
    public bool isLookingAtPlayer(float thresholdAngle)
    {
        Vector3 dir = (playerTargetNode.position - transform.position).normalized;

        float dotProd = Vector3.Dot(dir, transform.forward);

        return dotProd > thresholdAngle;
    }

    //Death
    void DeathEvent()
    {
        isDead = true;
        onDeath.Invoke();
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        startHealth = health;
        playerTargetNode = GameObject.FindGameObjectWithTag("PlayerTargetNode").transform;
        //Sanity Checks
        if (!(attacks.Count == attackDmg.Count && attackDmg.Count == attackRanges.Count))
        {
            Debug.LogError($"Attack Lists do not match on {gameObject.name}");
        }
        animator = GetComponent<Animator>();
        fireHead.SetActive(false);
        vfxCtrl = GetComponent<VFXController>();
        onStart.Invoke();
    }

    private void FixedUpdate()
    {
        if (!sleepOveride)
        {

            animator.SetBool("STOP", false);
            vfxBodyAnimatior.SetBool("STOP", false);

            //UI
            float ghostAmount = vfxCtrl.Progress(thresholdBeforeUnlock);
            float healthAmount = (health / startHealth);

            //if (healthAmount < ghostAmount)
            //{
            //    health = startHealth - (startHealth * healthAmount);
            //}

            healthUI.fillAmount = healthAmount;
            ghostUI.fillAmount = ghostAmount;
            if (ghostAmount <= 0.0f)
            {
                unlockedUI.enabled = true;
                lockedUI.enabled = false;
            }
            else
            {
                unlockedUI.enabled = false;
                lockedUI.enabled = true;
            }


            //Are we dead
            if (health <= 0)
            {
                if (!isDead)
                {
                    animator.SetTrigger("Death");
                    vfxBodyAnimatior.SetTrigger("Death");
                    DeathEvent();
                }
                //Leave loop early
                return;
            }

            //Health effects
            if (health < (startHealth * 0.5f))
            {
                if (!fireHead.activeInHierarchy)
                {
                    fireHead.SetActive(true);
                }
                agent.speed = movementSpeedAgro;
                agent.angularSpeed = turnSpeedAgro;
            }
            else
            {
                agent.speed = movementSpeed;
                agent.angularSpeed = turnSpeed;
            }



#if UNITY_EDITOR
            //DEBUGGING
            if (debugMode)
            {
                if (isLookingAtPlayer())
                {
                    Debug.DrawLine(eyes.position, playerTargetNode.position, Color.green);
                }
                else
                {
                    Debug.DrawLine(eyes.position, playerTargetNode.position, Color.yellow);
                }
            }
#endif
        }
        else
        {
            stopMovement();
            animator.SetBool("STOP", true);
            vfxBodyAnimatior.SetBool("STOP", true);
        }

    }

    //I was hit by something
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttackSurface"))
        {
            //Get Hurt
            health -= pc.umbreallaDmg;
            onHurt.Invoke();
        }
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            //Draw target
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target, 0.3f);
            //Draw last know player pos
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastKnownPlayerPosition, 0.3f);
        }
    }

#endif

}
