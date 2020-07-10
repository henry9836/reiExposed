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
    Vector3 tmp;

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

        //Reset agro mode
        ec.aggresiveMode = false;

        //Reset States And Triggers
        animator.SetBool("Idle", true);
        animator.SetBool("AttackMode", false);
        animator.SetBool("LosingPlayer", false);

        //Attacks Reset
        animator.ResetTrigger("Block");
        animator.ResetTrigger("LosingPlayer");

        //Random Wait Time
        waitTime = Random.Range(ec.stayAfterArrivalTimeRange.x, ec.stayAfterArrivalTimeRange.y);
        waitTimer = 0.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //DEBUGGING
#if UNITY_EDITOR
        ec.updateCurrentMode("IDLE");
#endif

        //If we can see the player then it time to attack 
        if (ec.canSeePlayer())
        {
            ec.stopMovement();
            animator.SetBool("AttackMode", true);
        }

        waitTimer += Time.deltaTime;

        if (waitTimer >= waitTime)
        {
            //Go somewhere new
            if (GetNewWanderTarget())
            {
                animator.SetBool("Idle", false);
                waitTimer = 0.0f;
            }
        }
    }

    bool GetNewWanderTarget()
    {
        tmp = startingLoc + new Vector3(Random.Range(-wanderRange, wanderRange), 0.0f, Random.Range(-wanderRange, wanderRange));
        return ec.GoToNewWanderPos(tmp);
    }

}
