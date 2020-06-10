using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameHeavyAttack : StateMachineBehaviour
{
    public Vector2 damageWindow;
    public AttackHost ah;
    public float visualTrigger = 0.5f;

    bool armed = false;
    bool armedVis = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ah == null)
        {
            ah = animator.gameObject.GetComponent<AttackHost>();
        }
        if (ah == null) { return; }
        ah.arm(true);
        armed = false;
        armedVis = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ah == null) { return; }
        //Turn on triggers
        if (damageWindow.y >= stateInfo.normalizedTime && stateInfo.normalizedTime > damageWindow.x && !armed)
        {
            ah.arm(true);
            armed = true;
        }
        //Turn off triggers
        else if (armed && damageWindow.y <= stateInfo.normalizedTime)
        {
            ah.arm(false);
            armed = false;
            ah.resetVisuals();
        }

        if (stateInfo.normalizedTime > visualTrigger && !armedVis) {
            ah.triggerVisuals(AttackHost.ATTACKS.SLAM);
            armedVis = true;
        }

        //Attack!
        ah.Attack(AttackHost.ATTACKS.SLAM);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (armed)
        {
            ah.arm(false);
            ah.resetVisuals();
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
