using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : StateMachineBehaviour
{
    public Vector2 attackWindow = new Vector2(0.0f, 0.9f);

    private umbrella umbrella;
    private bool once = true;
   

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (umbrella == null)
        {
            umbrella = animator.GetComponent<umbrella>();
        }
        animator.SetBool("Attacking", true);
        once = true;
        animator.ResetTrigger("Attack");
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (once == true)
        {
            if ((stateInfo.normalizedTime % 1.0f) >= attackWindow.x)
            {
                umbrella.Hitbox(true);
                once = false;
            }
        }


        if ((stateInfo.normalizedTime % 1.0f) >= attackWindow.y)
        {
            animator.SetBool("Attacking", false);
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        umbrella.Hitbox(false);
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
