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
    public LayerMask sightObstacles;

    [Header("Movement Settings")]
    public float movementSpeed = 10.0f;

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
#if UNITY_EDITOR
    //Debugging Hidden
    [HideInInspector]
    public Vector3 targetPos;
    [HideInInspector]
    public float Wanderrange;
    [HideInInspector]
    public Vector3 startingLoc;
#endif

    public void updateCurrentMode(string newMode)
    {
        currentMode = newMode;
    }

    //Go to a new position
    public void GoToTargetPos(GameObject target)
    {
        GoToTargetPos(target.transform.position);
    }

    public void GoToTargetPos(Vector3 target)
    {
        agent.isStopped = true;
        agent.SetDestination(target);
        agent.isStopped = false;
    }

    //Are we looking at the player?
    public bool isLookingAtPlayer() { return isLookingAtPlayer(thresholdSightAngle); }
    public bool isLookingAtPlayer(float thresholdAngle)
    {
        Vector3 dir = (playerTargetNode.position - transform.position).normalized;

        //If we are we need to turn to face player stop agent so we don't tokyo drift
        float dotProd = Vector3.Dot(dir, transform.forward);

        return dotProd > thresholdAngle;
    }

    public bool canSeePlayer()
    {
        RaycastHit hit;

        if (Physics.Raycast(eyes.position, playerTargetNode.position, out hit, maxSpotDistance, sightObstacles)) { Debug.Log($"I hit {hit.collider.gameObject.name}"); }
        else { Debug.Log("No hit"); }

        return isLookingAtPlayer() && (Physics.Raycast(eyes.position, playerTargetNode.position, out hit, maxSpotDistance, sightObstacles)); 
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        aggresiveMode = false;
        startHealth = health;
        playerTargetNode = GameObject.FindGameObjectWithTag("PlayerTargetNode").transform;
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
            Gizmos.DrawSphere(targetPos, 0.3f);
            //Draw Wander Area
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(startingLoc, new Vector3(Wanderrange * 2.0f, 0.3f, Wanderrange * 2.0f));
        }
    }

#endif

}
