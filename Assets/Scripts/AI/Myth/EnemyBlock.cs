﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlock : StateMachineBehaviour
{

    Transform player;
    MythCollisionHandler collHandler;

    float fullBlockTime = 0.5f;
    float blockTimeout = 1.0f;
    float blocktimer = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        animator.gameObject.GetComponent<AIMovement>().stopMovement();
        animator.SetBool("Blocking", true);
        animator.ResetTrigger("Block");

        collHandler = animator.gameObject.GetComponent<MythCollisionHandler>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Handle Timeout
        blocktimer += Time.deltaTime;
        if (blockTimeout < blocktimer)
        {
            animator.ResetTrigger("Block");
            animator.SetBool("Blocking", false);
        }

        if (blocktimer > fullBlockTime)
        {
            collHandler.fullyBlocking = true;
        }

        //If player is away stop blocking
        if (Vector3.Distance(player.position, animator.transform.position) > 5.0f)
        {
            blocktimer = blockTimeout;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        collHandler.fullyBlocking = false;
        animator.SetBool("Blocking", false);
        blocktimer = 0.0f;
    }

}