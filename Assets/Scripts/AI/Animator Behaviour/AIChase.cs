using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : StateMachineBehaviour
{

    AIObject ai;
    AIMovement movement;
    AITracker tracker;
    AIForwardAnimator forwarder;

    AIAttackContainer attack;
    Transform player;

    float repickAttackThreshold = 5.0f;
    float wrongAttackChosenTimer = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<AIObject>();
        }
        if (ai == null)
        {
            return;
        }
        if (movement == null)
        {
            movement = ai.movement;
        }
        if (tracker == null)
        {
            tracker = ai.tracker;
        }
        if (forwarder == null)
        {
            if (animator.GetBehaviour<AIForwardAnimator>() != null)
            {
                forwarder = animator.GetBehaviour<AIForwardAnimator>();
            }
        }

        player = ai.player.transform;

        //Reset Triggers
        for (int i = 0; i < ai.attacks.Count; i++)
        {
            animator.ResetTrigger(ai.attacks[i].triggerName);
            if (forwarder != null)
            {
                forwarder.ResetTrigger(ai.attacks[i].triggerName);
            }
        }

        //If there is no attack bound
        if (ai.selectedAttack == null)
        {
            ai.selectAttack();
        }

        attack = ai.getSelectedAttack();
        wrongAttackChosenTimer = 0.0f;

    }

    Vector3 getBestPositionForAttack()
    {
        Vector3 result = Vector3.zero;
        Vector3 direction = (ai.transform.position - tracker.lastSeenPos).normalized;
        //Go to a position that is in the middle of the range of attack in relation to the player's last seen position
        result = tracker.lastSeenPos + (direction * ((attack.rangeForAttack.y - attack.rangeForAttack.x) * 0.5f));

        return result;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            return;
        }

        wrongAttackChosenTimer += Time.deltaTime;

        if (wrongAttackChosenTimer >= repickAttackThreshold)
        {
            movement.stopMovement();
            ai.selectAttack();
            attack = ai.getSelectedAttack();
            wrongAttackChosenTimer = 0.0f;
        }

        //If we are too far from the player to attack go to player
        if (Vector3.Distance(ai.transform.position, player.position) > attack.rangeForAttack.y)
        {
            movement.goToPosition(getBestPositionForAttack());
        }
        //Too close to attack pick new attack!
        else if (Vector3.Distance(ai.transform.position, player.position) < attack.rangeForAttack.x)
        {
            movement.stopMovement();
            ai.selectAttack();
            attack = ai.getSelectedAttack();
        }
        //Close enough to attack
        else
        {
            //Go to the in between location for attack
            //movement.goToPosition(getBestPositionForAttack());
            //Can we see the player?
            if (tracker.canSeePlayer()) {

                //Debug Statement :D
                //Debug.Log($"{(attack.mustFacePlayer && tracker.isFacingPlayer() && !attack.overrideTrackingVisionCone)} || {!attack.mustFacePlayer} || {(attack.mustFacePlayer && tracker.isFacingPlayer(attack.facePlayerThreshold) && attack.overrideTrackingVisionCone)}");
                //Do we need to face player to attack, if so do we use an override if so then compare to override other if we are using an overrride use default settings or if we don't care about facing the player
                if ((attack.mustFacePlayer && tracker.isFacingPlayer() && !attack.overrideTrackingVisionCone) || !attack.mustFacePlayer || (attack.mustFacePlayer && tracker.isFacingPlayer(attack.facePlayerThreshold) && attack.overrideTrackingVisionCone))
                {
                    //ATTACK
                    movement.stopMovement();
                    animator.SetTrigger(attack.triggerName);
                    if (forwarder != null)
                    {
                        forwarder.SetTrigger(attack.triggerName);
                    }
                }
                else
                {
                    movement.goToPosition(tracker.lastSeenPos);
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
