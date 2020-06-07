using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
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
        WEAPON1,
        WEAPON2
    }
    public enum MOVESETRESTRICTION
    {
        ALL,
        PASSIVE,
        AGGRO
    }
    public class attack
    {
        public string name;
        public float damage;
        public bool damageOnlyOnce;
        public MOVESETRESTRICTION moveType;
        public Vector2 range;

        public attack(string _name, Vector2 _range, float _dmg, MOVESETRESTRICTION type)
        {
            name = _name;
            range = _range;
            damage = _dmg;
            moveType = type;
        }
    }

    //Defined Values
    [Header("General Settings")]
    public bool aggresiveMode = false;
    public float health = 100.0f;
    [Range(0.0f, 1.0f)]
    public float thresholdSightAngle = 0.5f;
    public float maxSpotDistance = 100.0f;
    [Range(0.0f, 1.0f)]
    public float sixthSenseDistanceFraction = 0.3f;
    public float losePlayerTimeThreshold = 5.0f;
    public float stuckTimerThreshold = 5.0f;
    public float stuckVeloThreshold = 1.0f;
    public float informRange = 20.0f;
    public float wanderRange = 30.0f;
    public float seekWanderRange = 10.0f;
    public float regenSpeed = 0.0f;
    public LayerMask sightObstacles;
    
    [Header("Movement Settings")]
    public float movementSpeed = 10.0f;
    public float walkSpeed = 4.0f;
    public float runningSpeed = 15.0f;
    public Vector2 stayAfterArrivalTimeRange = new Vector2(0.0f, 7.0f);
    public float arriveDistanceThreshold = 1.0f;

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onHurt;
    public UnityEvent onStart;

    [Header("Body Parts")]
    public Transform eyes;
    public List<BoxCollider> leftArms = new List<BoxCollider>();
    public List<BoxCollider> rightArms = new List<BoxCollider>();
    public List<BoxCollider> leftLegs = new List<BoxCollider>();
    public List<BoxCollider> rightLegs = new List<BoxCollider>();
    public List<BoxCollider> otherBody = new List<BoxCollider>();
    public List<BoxCollider> weapon_1 = new List<BoxCollider>();
    public List<BoxCollider> weapon_2 = new List<BoxCollider>();

    [Header("Moveset")]
    public bool canBlock = true;
    public float jumpDistance = 20.0f;
    public List<string> attacks = new List<string>();
    public List<Vector2> attackRanges = new List<Vector2>();
    public List<float> attackDmg = new List<float>();
    public List<MOVESETRESTRICTION> attackType = new List<MOVESETRESTRICTION>();

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
    public Animator animator;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public int selectedAttack;
    [HideInInspector]
    public Vector3 wanderTarget;
    [HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public Vector3 startingLoc;
    [HideInInspector]
    public float losePlayerTimer;
    [HideInInspector]
    public int workerID;
    [HideInInspector]
    public MythWorkerUnion union;

    //Privates
    private float stuckTimer = 0.0f;
    private float sixthSenseDistance;
    private float restrictRecalcTime = 1.5f;
    private float restrictRecalcTimer = 0.0f;
    private attack currentAttack = null;
    private Vector3 lastKnownPlayerDir = Vector3.zero;
    private PlayerController pc;
    private int blockCount;
    private int blockCountThresholdBeforeAggro = 5;
    private float blockSubtractTime = 10.0f;
    private float blockSubtractTimer = 0.0f;
    private float maxHealth;

    //PLAYER DAMAGE QUERY
    public float QueryDamage()
    {
        if (currentAttack != null) {
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
            Debug.LogWarning("QUERY REQUEST RECEIVED BUT THERE IS NO CURRENT ATTACK ASSIGNED");
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
                    for (int i = 0; i < weapon_1.Count; i++)
                    {
                        weapon_1[i].enabled = arm;
                    }
                    for (int i = 0; i < weapon_2.Count; i++)
                    {
                        weapon_2[i].enabled = arm;
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
            case ATTACKSURFACES.WEAPON1:
                {
                    for (int i = 0; i < weapon_1.Count; i++)
                    {
                        weapon_1[i].enabled = arm;
                    }
                    break;
                }
            case ATTACKSURFACES.WEAPON2:
                {
                    for (int i = 0; i < weapon_2.Count; i++)
                    {
                        weapon_2[i].enabled = arm;
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
        //For all attacks
        for (int i = 0; i < attacks.Count; i++)
        {
            //Select next attack
            tmp = new attack(attacks[i], attackRanges[i], attackDmg[i], attackType[i]);
            //Is this attack valid with our mode
            switch (tmp.moveType)
            {
                case MOVESETRESTRICTION.ALL:
                    break;
                case MOVESETRESTRICTION.PASSIVE:
                    { 
                        //skip this attack
                        if (aggresiveMode)
                        {
                            continue;
                        }
                        break; 
                    }
                case MOVESETRESTRICTION.AGGRO:
                    {
                        //skip this attack
                        if (!aggresiveMode)
                        {
                            continue;
                        }
                        break;
                    }
                default:
                    {
                        Debug.LogWarning($"Unknown MoveType: {tmp.moveType}");
                        break;
                    }
            }


            //How close do we need to get to start attacking?
            testingDis = Mathf.Abs(distanceFromPlayer - tmp.range.x);
            //If this the shortest distance we have found?
            if (testingDis < shortestDistance)
            {
                shortestDistance = testingDis;
                currentAttack = tmp;
            }
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

    public void ChangeHealth(float amount)
    {
        health += amount;
        //Clamp
        health = Mathf.Clamp(health, -1.0f, startHealth);
    }

    public void updateCurrentMode(string newMode)
    {
        currentMode = newMode;
    }

    //Go to a new position
    public void GoToTargetPosRestricted(Vector3 _target)
    {
        Debug.Log("Go to the random seek pos");
        if (restrictRecalcTimer >= restrictRecalcTime)
        {
            Debug.Log("OK Boss");
            GoToTargetPos(_target);
            restrictRecalcTimer = 0.0f;
        }
    }
    public void GoToNewWanderPos(Vector3 _target)
    {
        if (restrictRecalcTimer >= restrictRecalcTime) {
            wanderTarget = _target;
            GoToTargetPos(_target);
            restrictRecalcTimer = 0.0f;
        }
    }
    public void GoToTargetPos(GameObject _target)
    {
        GoToTargetPos(_target.transform.position);
    }

    public void GoToTargetPos(Vector3 _target)
    {
        agent.isStopped = true;
        agent.SetDestination(_target);
        target = _target;
        agent.isStopped = false;
    }

    //Are we looking at the player?
    public bool isLookingAtPlayer() { return isLookingAtPlayer(thresholdSightAngle, false); }
    public bool isLookingAtPlayer(bool _override) { return isLookingAtPlayer(thresholdSightAngle, _override); }
    public bool isLookingAtPlayer(float thresholdAngle, bool overrideChecks)
    {
        Vector3 dir = (playerTargetNode.position - transform.position).normalized;

        float dotProd = Vector3.Dot(dir, transform.forward);

        //If we are not in combat mode or we are losing the player
        if (!animator.GetBool("AttackMode") || (losePlayerTimer > losePlayerTimeThreshold * 0.5f) || overrideChecks)
        {
            return dotProd > thresholdAngle;
        }
        return true;
    }

    public bool canSeePlayer()
    {
        bool hitPlayer = false;

        RaycastHit hit;
        Vector3 dirOfPlayerNode = (playerTargetNode.position - eyes.position).normalized;
        //If we not in attack mode or we can see the player in our main cone
        if (!animator.GetBool("AttackMode") || isLookingAtPlayer(true)) {
            if (Physics.Raycast(eyes.position, dirOfPlayerNode, out hit, maxSpotDistance, sightObstacles))
            {
                if (hit.collider.tag == "PlayerTargetNode")
                {
                    hitPlayer = true;
                }
            }
            return isLookingAtPlayer() && hitPlayer;
        }
        //In attack mode and can see the player in our attack circle
        else
        {
            //If the player is within our range larger range of vision
            if (Physics.Raycast(eyes.position, dirOfPlayerNode, out hit, sixthSenseDistance, sightObstacles))
            {
                if (hit.collider.tag == "PlayerTargetNode")
                {
                    hitPlayer = true;
                }
            }
            return hitPlayer;
        }

    }

    //Detection
    public bool lostPlayer()
    {
        return (losePlayerTimer >= losePlayerTimeThreshold);
    }

    //Death
    void DeathEvent()
    {
        isDead = true;
        onDeath.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        aggresiveMode = false;
        startHealth = health;
        playerTargetNode = GameObject.FindGameObjectWithTag("PlayerTargetNode").transform;
        startingLoc = transform.position;
        animator = GetComponent<Animator>();
        UpdateAttackSurface(ATTACKSURFACES.ALL, false, true);
        pc = player.GetComponent<PlayerController>();
        sixthSenseDistance = maxSpotDistance * sixthSenseDistanceFraction;
        maxHealth = health;
        //Sanity Checks
        if (!(attacks.Count == attackType.Count && attackType.Count == attackRanges.Count))
        {
            Debug.LogError($"Attack Lists do not match on {gameObject.name}");
        }

        onStart.Invoke();
    }

    private void FixedUpdate()
    {
        //Keep Track Of Player
        if (canSeePlayer())
        {
            lastKnownPlayerPosition = player.transform.position;
            lastKnownPlayerDir = player.GetComponent<movementController>().charcterModel.transform.forward;
            losePlayerTimer = 0.0f;
            union.ISeeThePlayer(workerID);
        }
        //Losing Player
        else
        {
            if (losePlayerTimer == 0.0f)
            {
                lastKnownPlayerPosition = lastKnownPlayerPosition + (lastKnownPlayerDir.normalized * 5.0f);
            }
            losePlayerTimer += Time.deltaTime; 
        }


        //Stuck Logic
        if (agent.velocity.magnitude < stuckVeloThreshold && !animator.GetBool("Idle"))
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0.0f;
        }
        //If we are stuck
        if (stuckTimer > stuckTimerThreshold)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"{gameObject.name} got stuck, attempting to fix...");
#endif
            stopMovement();
            animator.SetBool("Idle", true);
        }

        //Death
        if (health <= 0)
        {
            if (!isDead)
            {
                DeathEvent();
            }
        }

        //Stop a race condition
        restrictRecalcTimer += Time.deltaTime;

        //Change movement speed based on mode
        if (aggresiveMode)
        {
            agent.speed = runningSpeed;
        }
        else
        {
            agent.speed = walkSpeed;
        }

        //Restrict blocking
        blockSubtractTimer += Time.deltaTime;
        if (blockSubtractTimer >= blockSubtractTime)
        {
            blockCount--;
            if (blockCount < 0)
            {
                blockCount = 0;
            }
        }

        //Regen health
        if (!animator.GetBool("AttackMode"))
        {
            health += Time.deltaTime * regenSpeed;

            health = Mathf.Clamp(health, 0.0f, maxHealth);

        }

#if UNITY_EDITOR
        //DEBUGGING
        if (debugMode)
        {
            if (canSeePlayer())
            {
                Debug.DrawLine(eyes.position, playerTargetNode.position, Color.green);
            }
            else if (isLookingAtPlayer())
            {
                Debug.DrawLine(eyes.position, playerTargetNode.position, Color.yellow);
            }
            else
            {
                Debug.DrawLine(eyes.position, playerTargetNode.position, Color.red);
            }
        }
#endif
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            //Draw Wander Target
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(wanderTarget, 0.3f);
            //Draw target
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target, 0.3f);
            //Draw last know player pos
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastKnownPlayerPosition, 0.3f);
            //Draw Sixth Sense
            Gizmos.color = new Color(1.5f, 0.5f, 1.0f);
            Gizmos.DrawWireSphere(transform.position, sixthSenseDistance);
            //Draw Wander Area
            Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 0.3f);
            Gizmos.DrawCube(startingLoc, new Vector3(wanderRange * 2.0f, 0.3f, wanderRange * 2.0f));
        }
    }

#endif

    //I was hit by something
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttackSurface"))
        {
            if (!animator.GetBool("Blocking"))
            {

                bool blocked = false;
                if (!animator.GetBool("Attacking"))
                {
                    int coin = Random.Range(0, 100);
                    //High chance to block if in passive mode
                    if (!aggresiveMode)
                    {
                        blocked = coin < 50; //50% chance
                    }
                    //Low chance to block if in agro mode
                    else
                    {
                        blocked = coin < 10; //10% chance
                    }


                    if (!blocked)
                    {
                        //Get Hurt
                        stopMovement();
                        health -= pc.umbreallaDmg;
                        onHurt.Invoke();
                        animator.SetTrigger("Stun");
                    }
                    else
                    {
                        blockSubtractTimer = 0.0f;
                        blockCount++;
                        animator.SetTrigger("Block");
                    }

                    if (blockCount >= blockCountThresholdBeforeAggro)
                    {
                        aggresiveMode = true;
                    }

                }
                else
                {
                    //Get Hurt
                    health -= pc.umbreallaDmg;
                    onHurt.Invoke();
                }

                if (!animator.GetBool("AttackMode"))
                {
                    animator.SetBool("AttackMode", true);
                }

            }
        }
    }

}
