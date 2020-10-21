using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStunBehaviour : StateMachineBehaviour
{
    [Range(0.0f, 1.0f)]
    public float exitTrigger = 0.95f;

    umbrella umbrella;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (umbrella == null)
        {
            umbrella = animator.gameObject.GetComponent<umbrella>();
        }

        animator.ResetTrigger("KnockDown");
        animator.ResetTrigger("Stun");
        animator.ResetTrigger("KnockBack");
        animator.SetBool("Stunned", true);

        animator.SetInteger("StunToUse", Random.Range(0, 3));

        //Disable our hitbox
        umbrella.Hitbox(false);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if ((stateInfo.normalizedTime % 1.0f) >= exitTrigger)
        {
            animator.SetBool("Stunned", false);
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
