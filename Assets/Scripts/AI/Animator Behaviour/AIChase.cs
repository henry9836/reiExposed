using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : StateMachineBehaviour
{

    AIObject ai;
    AIMovement movement;
    AITracker tracker;

    AIAttackContainer attack;
    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<AIObject>();
        }
        if (movement == null)
        {
            movement = ai.movement;
        }
        if (tracker == null)
        {
            tracker = ai.tracker;
        }

        player = ai.player.transform;

        //If there is no attack bound
        if (ai.selectedAttack == null)
        {
            ai.selectAttack();
        }

        attack = ai.getSelectedAttack();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If we are too far from the player to attack go to player
        if (Vector3.Distance(ai.transform.position, player.position) > attack.rangeForAttack.y)
        {
            movement.goToPosition(tracker.lastSeenPos);
        }
        //Too close to attack pick new attack!
        else if (Vector3.Distance(ai.transform.position, player.position) < attack.rangeForAttack.x)
        {
            ai.selectAttack();
            attack = ai.getSelectedAttack();
        }
        //Close enough to attack
        else
        {
            //Can we see the player?
            if (tracker.canSeePlayer()) {
                //Do we need to face player to attack, if so do we use an override if so then compare to override other if we are using an overrride use default settings or if we don't care about facing the player
                if ((attack.mustFacePlayer && tracker.isFacingPlayer() && !attack.overrideTrackingVisionCone) || !attack.mustFacePlayer || (attack.mustFacePlayer && tracker.isFacingPlayer(attack.facePlayerThreshold) && attack.overrideTrackingVisionCone))
                {
                    //ATTACK
                    movement.stopMovement();
                    animator.SetTrigger(attack.triggerName);
                }
            }
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
