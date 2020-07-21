using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDash : StateMachineBehaviour
{
    [Range(0.0f, 1.0f)]
    public float dashTrigger = 0.1f;
    [Range(0.0f, 1.0f)]
    public float losePlayerTrigger = 0.5f;
    [Range(0.0f, 1.0f)]
    public float endDashTrigger = 0.9f;
    public float dashSpeedMulti = 2.0f;
    public float playerStopTheshold = 1.0f;
    public LayerMask obsctucles;
    
    float dashSpeed = 10.0f;
    bool dashing = false;
    bool lostPlayerTracking = false;
    AIObject ai;
    AITracker tracker;
    AIMovement movement;
    AIForwardAnimator forwarder;
    Transform transform;
    Vector3 targetDir;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<AIObject>();
            if (ai == null)
            {
                return;
            }
        }

        if (tracker == null)
        {
            tracker = ai.tracker;
        }
        if (movement == null)
        {
            movement = ai.movement;
        }
        if (forwarder == null)
        {
            if (animator.GetBehaviour<AIForwardAnimator>() != null)
            {
                forwarder = animator.GetBehaviour<AIForwardAnimator>();
            }
        }

        dashing = false;
        lostPlayerTracking = false;

        dashSpeed = movement.moveSpeed * dashSpeedMulti;
        transform = ai.transform;

        movement.stopMovement();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            return;
        }

        movement.goToPosition(tracker.lastSeenPos);

        if (!dashing && ((stateInfo.normalizedTime % 1.0f) >= dashTrigger))
        {
            dashing = true;
            movement.setOverride(AIMovement.OVERRIDE.MOVE_OVERRIDE);
        }
        if (!lostPlayerTracking && ((stateInfo.normalizedTime % 1.0f) >= losePlayerTrigger))
        {
            lostPlayerTracking = true;
            movement.setOverride(AIMovement.OVERRIDE.FULL_OVERRIDE);
        }

        //Dash forwards
        if (dashing)
        {
            //If we are close enought to the player stop moving
            if (playerStopTheshold >= Vector3.Distance(transform.position, ai.player.transform.position))
            {
                dashing = false;
                return;
            }

            //Update Dash Direction
            if (!lostPlayerTracking)
            {
                //Get Direction
                targetDir = (tracker.lastSeenPos - transform.position).normalized;
                //Remove height
                targetDir.y *= 0.0f;
            }


            //Check for obsticles
           Vector3 offset = targetDir * dashSpeed * Time.deltaTime;
            float dashDistance = Vector3.Distance(transform.position, transform.position + offset);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, targetDir, out hit, dashDistance, obsctucles))
            {
                //Move up until the rayhit point
                transform.position += offset - (targetDir * hit.distance);
                dashing = false;
                return;
            }
            else
            {
                //Move in a direction that makes sense
                transform.position += offset;
            }

        }

        //Ends on animation over
        if ((stateInfo.normalizedTime % 1.0f) >= endDashTrigger)
        {
            forwarder.SetBool("Dashing", false);
            animator.SetBool("Dashing", false);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (movement != null)
        {
            movement.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
        }
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
