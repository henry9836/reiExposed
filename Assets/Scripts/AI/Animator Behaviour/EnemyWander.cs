using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : StateMachineBehaviour
{
    GameObject enemy;
    EnemyController ec;
    GameObject player;
    public float arriveDistanceThreshold = 1.0f;
    public float stuckTimerThreshold = 5.0f;
    public float stuckVeloThreshold = 1.0f;
    public float informRange = 20.0f;
    public float wanderRange = 30.0f;
    public float regenSpeed = 0.0f;
    public Vector2 stayTimeRange = new Vector2(0.0f, 7.0f);
    public List<string> attacks = new List<string>();
    public List<Vector2> attackRanges = new List<Vector2>();

    private Vector3 wanderTarget = Vector3.zero;
    private Vector3 startingLocation = Vector3.zero;
    private float stuckTimer = 0.0f;
    private float stayTime = 0.0f;
    private float stayTimer = 0.0f;
    private float recalcTimer = 0.0f;
    private float maxTimeBeforeAllowedRecalc = 0.5f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enemy)
        {
            enemy = animator.gameObject;
        }
        if (ec == null)
        {
            ec = enemy.GetComponent<EnemyController>();
        }
        if (!player)
        {
            player = ec.player;
        }
        if (startingLocation == Vector3.zero)
        {
            startingLocation = enemy.transform.position;
        }

        GetNewWanderTarget();
        stuckTimer = 0.0f;
        stayTimer = 0.0f;
        stayTime = Random.Range(stayTimeRange.x, stayTimeRange.y);

#if UNITY_EDITOR
        if (attacks.Count != attackRanges.Count)
        {
            Debug.LogError($"Attack List Lengths Do Not Match On {animator.gameObject.name}");
        }
#endif
    }


    void GetNewWanderTarget()
    {
        if (recalcTimer >= maxTimeBeforeAllowedRecalc)
        {
            //Assign a new wander target
            wanderTarget = startingLocation + new Vector3(Random.Range(-wanderRange, wanderRange), 0.0f, Random.Range(-wanderRange, wanderRange));
            ec.GoToTargetPos(wanderTarget);
            recalcTimer = 0.0f;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        recalcTimer += Time.deltaTime;

        //If we can see the player
        if (ec.canSeePlayer())
        {
#if UNITY_EDITOR
            ec.currentMode = "ATTACKING";
#endif
            //Inform my neighbours of the attacking player


            //Attack player
            ec.agent.SetDestination(player.transform.position);


        }

        //If we cannot see the player wander in the scene
        else
        {
#if UNITY_EDITOR
            ec.currentMode = "WANDERING";
#endif
            //Heal
            ec.ChangeHealth(regenSpeed * Time.deltaTime);

            //If we have reached our wander direction
            if (Vector3.Distance(ec.transform.position, wanderTarget) <= arriveDistanceThreshold)
            {
                stayTimer += Time.deltaTime;
                if (stayTimer >= stayTime)
                {
                    stayTime = Random.Range(stayTimeRange.x, stayTimeRange.y);
                    GetNewWanderTarget();
                    stayTimer = 0.0f;
                }
            }
            //If we are currently not moving to a target
            else if(ec.agent.velocity.magnitude < stuckVeloThreshold)
            {
                stuckTimer += Time.deltaTime;
            }
            //If we are moving towards our target
            else
            {
                stuckTimer = 0.0f;
            }

            //If we have been stuck for a while find a new wander target
            if (stuckTimer > stuckTimerThreshold)
            {
#if UNITY_EDITOR
                ec.currentMode = "STUCK";
#endif
                GetNewWanderTarget();
            }
        }

#if UNITY_EDITOR
        ec.targetPos = wanderTarget;
        ec.Wanderrange = wanderRange;
        ec.startingLoc = startingLocation;
#endif

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

}
