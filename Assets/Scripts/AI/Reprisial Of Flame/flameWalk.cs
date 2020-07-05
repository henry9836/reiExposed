using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameWalk : StateMachineBehaviour
{
    public float dashDistanceTheshold = 10.0f;

    Transform player;
    Transform transform;
    Transform boss;
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
        if (rc == null || !rc.enabled) { return; }
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

        currentAttack = rc.getAttack();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If we are the VFX animator
        if (rc == null || !rc.enabled) { return; }

        rc.GoToTargetPos(player.position);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance >= currentAttack.range.x && distance < currentAttack.range.y)
        {
            animator.SetBool("Idle", true);
            vfxAnimator.SetBool("Idle", true);
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
