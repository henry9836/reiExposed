using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readyShoot : StateMachineBehaviour
{
    private umbrella umbrella;
    private bool once = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (umbrella == null)
        {
            umbrella = GameObject.FindGameObjectWithTag("Player").GetComponent<umbrella>();
        }
        umbrella.canfire = false;
        umbrella.ISBLockjing = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && !once)
        {
            umbrella.canfire = true;
            umbrella.ISBLockjing = true;
            once = true;
        }
        else if (!once)
        {
            umbrella.canfire = false;
            umbrella.ISBLockjing = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        once = false;
        umbrella.canfire = false;
        umbrella.ISBLockjing = false;
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
