using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readyShoot : StateMachineBehaviour
{
    public GameObject umbrella;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        umbrella = GameObject.Find("umbrella ella ella");
        umbrella.GetComponent<umbrella>().canfire = false;
        umbrella.GetComponent<umbrella>().ISBLockjing = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            umbrella.GetComponent<umbrella>().canfire = true;
            umbrella.GetComponent<umbrella>().ISBLockjing = true;
        }
        else
        {
            umbrella.GetComponent<umbrella>().canfire = false;
            umbrella.GetComponent<umbrella>().ISBLockjing = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        umbrella.GetComponent<umbrella>().canfire = false;
        umbrella.GetComponent<umbrella>().ISBLockjing = false;
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
