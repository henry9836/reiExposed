﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : StateMachineBehaviour
{
    GameObject enemy;
    EnemyController ec;
    GameObject player;
    Vector3 startingLoc;

    EnemyController.attack currentAttack;

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

        //Get our attack
        currentAttack = ec.getAttack();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //DEBUGGING
#if UNITY_EDITOR
        ec.updateCurrentMode("CHASING");
#endif
        //Are we close enough to the player to start our attack?
        if (((ec.lastKnownPlayerPosition - enemy.transform.position).sqrMagnitude > currentAttack.range.x) && ((ec.lastKnownPlayerPosition - enemy.transform.position).sqrMagnitude <= currentAttack.range.y))
        {
            //Lost player?
            animator.SetBool("LosingPlayer", !ec.canSeePlayer());

            //We have not lost player
            if (!ec.lostPlayer()) {
                //Attack
                animator.SetBool("Idle", true);
                ec.stopMovement();
            }
        }
        else
        {
            ec.GoToTargetPos(ec.lastKnownPlayerPosition);
        }

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