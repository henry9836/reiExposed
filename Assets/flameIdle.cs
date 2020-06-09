using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameIdle : StateMachineBehaviour
{

    Transform player;
    Transform transform;
    ReprisialOfFlameController rc;
    ReprisialOfFlameController.attack currentAttack;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rc == null)
        {
            rc = animator.gameObject.GetComponent<ReprisialOfFlameController>();
        }
        if (player == null)
        {
            player = rc.player.transform;
        }
        if (transform == null)
        {
            transform = rc.transform;
        }

        //Reset values
        animator.ResetTrigger("BasicAttack");
        animator.ResetTrigger("FlameHead");
        animator.ResetTrigger("HeavyAttack");

        animator.SetBool("Dashing", false);
        animator.SetBool("AOEStomp", false);
        animator.SetBool("FlameAssing", false);

        //Stop if we are moving
        rc.stopMovement();

        //Pick a new attack if there is none
        if (rc.hasAttack())
        {
            currentAttack = rc.getAttack();
        }
        else
        {
            currentAttack = rc.pickAttack();
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(transform.position, player.position);

        //Are close enough to do our attack?
        if (distance >= currentAttack.range.x && distance < currentAttack.range.y)
        {
            if (currentAttack.attackIsBool)
            {
                animator.SetBool(currentAttack.name, true);
            }
            else
            {
                animator.SetTrigger(currentAttack.name);
            }
        }
        //Go to player until we reach the distance we want
        else
        {
            animator.SetBool("Idle", false);
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
