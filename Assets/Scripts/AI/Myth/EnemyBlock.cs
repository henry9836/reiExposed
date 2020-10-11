using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlock : StateMachineBehaviour
{

    Transform player;
    MythCollisionHandler collHandler;
    AIMovement movementCtrl;
    AITracker tracker;
    AIObject ai;

    public Vector2 fullBlockTimeRange = new Vector2(1.0f, 10.0f);
    float fullBlockTime = 1.0f;
    float blocktimer = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            movementCtrl = animator.gameObject.GetComponent<AIMovement>();
            tracker = animator.gameObject.GetComponent<AITracker>();
            ai = animator.gameObject.GetComponent<AIObject>();
        }

        animator.gameObject.GetComponent<AIMovement>().stopMovement();
        animator.SetBool("Blocking", true);
        animator.ResetTrigger("Block");

        collHandler = animator.gameObject.GetComponent<MythCollisionHandler>();

        fullBlockTime = Random.Range(fullBlockTimeRange.x, fullBlockTimeRange.y);

        movementCtrl.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
        movementCtrl.setOverride(AIMovement.OVERRIDE.MOVE_OVERRIDE);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Handle Timeout
        blocktimer += Time.deltaTime;

        //If we are not facing the player turn to face player
        if (!tracker.isFacingPlayer())
        {
            //movementCtrl.goToPosition(tracker.lastSeenPos);
            animator.ResetTrigger("Block");
            animator.SetBool("Blocking", false);
        }

        if (blocktimer > fullBlockTime)
        {
            collHandler.fullyBlocking = true;
            animator.ResetTrigger("Block");
            animator.SetBool("Blocking", false);
            //Whack randomly
            int coin = Random.Range(0, 11);
            if (coin >= 5)
            {
                animator.SetTrigger("Whack");
                ai.bindAttack("Whack");
            }
        }

        //If player is away stop blocking
        if (Vector3.Distance(player.position, animator.transform.position) > 5.0f)
        {
            blocktimer = fullBlockTime;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        collHandler.fullyBlocking = false;
        animator.SetBool("Blocking", false);
        animator.SetBool("Attacking", false);
        animator.ResetTrigger("Block");
        blocktimer = 0.0f;
        movementCtrl.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
    }

}
