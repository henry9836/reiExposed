﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackEvent : StateMachineBehaviour
{
    
    public EnemyController.ATTACKSURFACES attackSurfaces;
    public Vector2 damageWindow;
    public bool damageOnlyOnce;

    EnemyController ec;
    bool armed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attacking", true);

        if (ec == null)
        {
            ec = animator.gameObject.GetComponent<EnemyController>();
        }
        ec.stopMovement();
        armed = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Turn on triggers
        if (damageWindow.y >= stateInfo.normalizedTime && stateInfo.normalizedTime > damageWindow.x && !armed)
        {
            ec.UpdateAttackSurface(attackSurfaces, true, damageOnlyOnce);
            armed = true;
        }
        //Turn off triggers
        else if (armed && damageWindow.y <= stateInfo.normalizedTime)
        {
            ec.UpdateAttackSurface(attackSurfaces, false, damageOnlyOnce);
            armed = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (armed)
        {
            ec.UpdateAttackSurface(attackSurfaces, false, damageOnlyOnce);
        }

        animator.SetBool("Attacking", false);
    }
}