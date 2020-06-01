using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeek : StateMachineBehaviour
{

    GameObject enemy;
    EnemyController ec;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Init
        if (!enemy)
        {
            enemy = animator.gameObject;
        }
        if (ec == null)
        {
            ec = enemy.GetComponent<EnemyController>();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
#if UNITY_EDITOR
        ec.updateCurrentMode("SEARCHING");
#endif
        //If we have lost the player leave
        if (ec.lostPlayer())
        {
            //Leave to next state
            animator.SetBool("LosingPlayer", false);
            animator.SetTrigger("LostPlayer");
        }
        //There you are
        else if (ec.canSeePlayer())
        {
            //Leave to next state
            animator.SetBool("LosingPlayer", false);
        }
        else
        {
            //Explore around the place 
            ec.GoToTargetPos(ec.lastKnownPlayerPosition + new Vector3(Random.Range(-ec.seekWanderRange, ec.seekWanderRange), 0.0f, Random.Range(-ec.seekWanderRange, ec.seekWanderRange)));

            //Leave to next state
            animator.SetBool("Idle", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ec.lostPlayer())
        {
            animator.SetBool("AttackMode", false);
        }
        animator.ResetTrigger("LostPlayer");
    }

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
