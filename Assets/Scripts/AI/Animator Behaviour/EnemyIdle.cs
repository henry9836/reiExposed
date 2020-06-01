using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : StateMachineBehaviour
{

    GameObject enemy;
    EnemyController ec;
    GameObject player;
    Vector3 startingLoc;

    float waitTime = 0.0f;
    float waitTimer = 0.0f;
    float wanderRange = 0.0f;

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
        if (startingLoc == Vector3.zero)
        {
            startingLoc = ec.startingLoc;
        }
        if (wanderRange == 0.0f)
        {
            wanderRange = ec.wanderRange;
        }

        //Reset States And Triggers
        animator.SetBool("Idle", true);
        animator.SetBool("AttackMode", false);
        animator.SetBool("LosingPlayer", false);

        //Attacks Reset
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Block");
        animator.ResetTrigger("LosingPlayer");

        //Random Wait Time
        waitTime = Random.Range(ec.stayAfterArrivalTimeRange.x, ec.stayAfterArrivalTimeRange.y);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //DEBUGGING
#if UNITY_EDITOR
        ec.updateCurrentMode("IDLE");
#endif

        waitTimer += Time.deltaTime;

        if (waitTimer >= waitTime)
        {
            //Go somewhere new
            GetNewWanderTarget();
            animator.SetBool("Idle", false);
        }
    }

    void GetNewWanderTarget()
    {
        ec.wanderTarget = startingLoc + new Vector3(Random.Range(-wanderRange, wanderRange), 0.0f, Random.Range(-wanderRange, wanderRange));
        ec.GoToTargetPos(ec.wanderTarget);
    }

}
