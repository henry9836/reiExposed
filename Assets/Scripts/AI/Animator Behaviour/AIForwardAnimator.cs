using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIForwardAnimator : StateMachineBehaviour
{

    Animator otherAnimator;

    public void ResetTrigger(string trigger)
    {
        otherAnimator.ResetTrigger(trigger);
    }

    public void SetTrigger(string trigger)
    {
        otherAnimator.SetTrigger(trigger);
    }

    public void SetBool(string boolName, bool state)
    {
        otherAnimator.SetBool(boolName, state);
    }
    public void SetFloat(string name, int value)
    {
        otherAnimator.SetFloat(name, value);
    }

    public void SetInteger(string name, int value)
    {
        otherAnimator.SetInteger(name, value);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (otherAnimator == null)
        {
            otherAnimator = animator.gameObject.GetComponent<AIObject>().forwardAnimationsTo;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
