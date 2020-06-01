using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class EnemyController : MonoBehaviour
{
    //Defined Values
    [Header("General Settings")]
    public bool aggresiveMode = false;
    public float health = 100.0f;
    [Range(0.0f, 1.0f)]
    public float thresholdSightAngle = 0.5f;
    public float maxSpotDistance = 100.0f;
    public float losePlayerTimeThreshold = 5.0f;
    public float stuckTimerThreshold = 5.0f;
    public float stuckVeloThreshold = 1.0f;
    public float informRange = 20.0f;
    public float wanderRange = 30.0f;
    public float regenSpeed = 0.0f;
    public LayerMask sightObstacles;


    [Header("Movement Settings")]
    public float movementSpeed = 10.0f;
    public Vector2 stayAfterArrivalTimeRange = new Vector2(0.0f, 7.0f);
    public float arriveDistanceThreshold = 1.0f;
    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onStart;

    [Header("Body Parts")]
    public Transform eyes;
    public List<BoxCollider> leftArms = new List<BoxCollider>();
    public List<BoxCollider> rightArms = new List<BoxCollider>();
    public List<BoxCollider> leftLegs = new List<BoxCollider>();
    public List<BoxCollider> rightLegs = new List<BoxCollider>();
    public List<BoxCollider> otherBody = new List<BoxCollider>();

    [Header("Moveset")]
    public bool canBlock = true;
    public List<string> attacks = new List<string>();
    public List<Vector2> attackRanges = new List<Vector2>();

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
    public Vector3 lastKnownPlayerPos;
    [HideInInspector]
    public int selectedAttack;
    [HideInInspector]
    public Vector3 wanderTarget;
    [HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public Vector3 startingLoc;

    //Privates
    private float stuckTimer = 0.0f;

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
    public bool isLookingAtPlayer() { return isLookingAtPlayer(thresholdSightAngle); }
    public bool isLookingAtPlayer(float thresholdAngle)
    {
        Vector3 dir = (playerTargetNode.position - transform.position).normalized;

        float dotProd = Vector3.Dot(dir, transform.forward);

        return dotProd > thresholdAngle;
    }

    public bool canSeePlayer()
    {
        bool hitPlayer = false;

        RaycastHit hit;
        Vector3 dirOfPlayerNode = (playerTargetNode.position - eyes.position).normalized;
        if (Physics.Raycast(eyes.position, dirOfPlayerNode, out hit, maxSpotDistance, sightObstacles)) 
        { 
            if (hit.collider.tag == "PlayerTargetNode")
            {
                hitPlayer = true;
            }
        }

        return isLookingAtPlayer() && hitPlayer; 
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
        onStart.Invoke();
    }

    //Death
    void DeathEvent()
    {
        isDead = true;
        onDeath.Invoke();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {

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
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(wanderTarget, 0.3f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target, 0.3f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastKnownPlayerPos, 0.3f);
            //Draw Wander Area
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(startingLoc, new Vector3(wanderRange * 2.0f, 0.3f, wanderRange * 2.0f));
        }
    }

#endif

}
