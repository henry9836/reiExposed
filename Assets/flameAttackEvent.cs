using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameAttackEvent : StateMachineBehaviour
{
    public ReprisialOfFlameController.ATTACKSURFACES attackSurfaces;
    public Vector2 damageWindow;
    public bool damageOnlyOnce;

    ReprisialOfFlameController rc;
    bool armed = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rc == null)
        {
            rc = animator.gameObject.GetComponent<ReprisialOfFlameController>();
        }

        rc.stopMovement();
        armed = false;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Turn on triggers
        if (damageWindow.y >= stateInfo.normalizedTime && stateInfo.normalizedTime > damageWindow.x && !armed)
        {
            rc.UpdateAttackSurface(attackSurfaces, true, damageOnlyOnce);
            armed = true;
        }
        //Turn off triggers
        else if (armed && damageWindow.y <= stateInfo.normalizedTime)
        {
            rc.UpdateAttackSurface(attackSurfaces, false, damageOnlyOnce);
            armed = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (armed)
        {
            rc.UpdateAttackSurface(attackSurfaces, false, damageOnlyOnce);
        }
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
