﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armBodyPartsForAttack : StateMachineBehaviour
{

    public Vector2 damageWindow = new Vector2(0.0f, 1.0f);
    public BossController.ARMTYPE bodyPartsToArm;
    public float attackDamage = 5.0f;
    public bool onlyApplyDamageOnce = true;

    private bool armed = false;

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (damageWindow.y >= stateInfo.normalizedTime && stateInfo.normalizedTime > damageWindow.x && !armed)
        {
            animator.gameObject.GetComponent<BossController>().animationOverrideFunc(true);
            animator.gameObject.GetComponent<BossController>().arm(bodyPartsToArm, true, attackDamage, onlyApplyDamageOnce);
            armed = true;
        }
        else if (armed && damageWindow.y <= stateInfo.normalizedTime)
        {
            animator.gameObject.GetComponent<BossController>().animationOverrideFunc(false);
            animator.gameObject.GetComponent<BossController>().arm(bodyPartsToArm, false, attackDamage, onlyApplyDamageOnce);
            armed = false;
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