using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameIdle : StateMachineBehaviour
{

    public float dashDistanceTheshold = 10.0f;

    Transform player;
    Transform boss;
    Transform transform;
    Animator vfxAnimator;
    ReprisialOfFlameController rc;
    ReprisialOfFlameController.attack currentAttack;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rc == null)
        {
            rc = animator.gameObject.GetComponent<ReprisialOfFlameController>();
        }
        //If we are the VFX animator
        if (rc == null) { return; }
        if (player == null)
        {
            player = rc.player.transform;
        }
        if (transform == null)
        {
            transform = rc.transform;
        }
        if (vfxAnimator == null)
        {
            vfxAnimator = rc.vfxBodyAnimatior;
        }
        if (boss == null)
        {
            boss = animator.gameObject.transform;
        }

        //Reset values
        animator.ResetTrigger("BasicAttack");
        animator.ResetTrigger("FlameHead");
        animator.ResetTrigger("HeavyAttack");
        vfxAnimator.ResetTrigger("BasicAttack");
        vfxAnimator.ResetTrigger("FlameHead");
        vfxAnimator.ResetTrigger("HeavyAttack");

        animator.SetBool("Dashing", false);
        animator.SetBool("AOEStomp", false);
        animator.SetBool("FlameAssing", false);
        vfxAnimator.SetBool("Dashing", false);
        vfxAnimator.SetBool("AOEStomp", false);
        vfxAnimator.SetBool("FlameAssing", false);

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
        //If we are the VFX animator
        if (rc == null) { return; }

        float distance = Vector3.Distance(transform.position, player.position);

        //Are close enough to do our attack and facing the player?
        if (distance >= currentAttack.range.x && distance < currentAttack.range.y && rc.isLookingAtPlayer(0.2f))
        {
            if (currentAttack.attackIsBool)
            {
                vfxAnimator.SetBool(currentAttack.name, true);
                animator.SetBool(currentAttack.name, true);
            }
            else
            {
                vfxAnimator.SetTrigger(currentAttack.name);
                animator.SetTrigger(currentAttack.name);
            }
        }
        //Go to player until we reach the distance we want
        else
        {
            vfxAnimator.SetBool("Idle", false);
            animator.SetBool("Idle", false);
        }

        //Dash To Player if too far
        if (dashDistanceTheshold <= Vector3.Distance(player.position, boss.position))
        {
            animator.SetBool("Dashing", true);
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
