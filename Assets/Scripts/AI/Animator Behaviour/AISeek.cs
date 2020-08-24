using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISeek : StateMachineBehaviour
{

    AITracker tracker;
    AIObject ai;
    AIMovement movement;
    AIForwardAnimator forwarder;

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
        if (forwarder == null)
        {
            if (animator.GetBehaviour<AIForwardAnimator>() != null)
            {
                forwarder = animator.GetBehaviour<AIForwardAnimator>();
            }
        }
        //Go to where we think player will be
        movement.goToPosition(tracker.estimateNewPosition());

        animator.ResetTrigger("Inform");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            return;
        }
        //Once we have arrived to a location pick a new position in last known area
        if (movement.agentArrived())
        {
            movement.goToPosition(tracker.predictedPlayerPos + new Vector3(Random.Range(-tracker.seekWanderRange, tracker.seekWanderRange), 0.0f, Random.Range(-tracker.seekWanderRange, tracker.seekWanderRange)));
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
