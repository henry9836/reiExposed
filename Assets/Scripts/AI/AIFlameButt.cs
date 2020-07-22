using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlameButt : StateMachineBehaviour
{
    [Range(0.0f, 1.0f)]
    public float attackTrigger = 0.35f;

    AIObject ai;
    AITracker tracker;
    Transform player;
    AIBody body;
    AIMovement movement;
    AIFlameButtController butt;
    bool attacked = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<AIObject>();
            if (ai == null)
            {
                return;
            }
            movement = ai.movement;
        }

        butt = ai.gameObject.GetComponentInChildren<AIFlameButtController>();

        movement.stopMovement();
        movement.setOverride(AIMovement.OVERRIDE.FULL_OVERRIDE);

        attacked = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!attacked && ((stateInfo.normalizedTime % 1.0f) >= attackTrigger)){
            attacked = true;
            butt.flameItUp();
            animator.ResetTrigger("FlameButt");
            animator.SetBool("Attacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movement.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
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
