using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWander : StateMachineBehaviour
{

    AIObject ai;
    AIMovement movement;

    Vector3 wanderTarget;
    bool pickedTarget;

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

        pickedTarget = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!pickedTarget)
        {
            //Pick a random spot within wandering range from inital pos
            wanderTarget = movement.pickWanderPosition();
            //Can reach destination
            if (movement.canReachDest(wanderTarget))
            {
                //Go to target
                movement.goToPosition(wanderTarget);
                pickedTarget = true;
            }
        }
        else
        {
            //If we are close enough to our destination stop
            if (Vector3.Distance(ai.transform.position, wanderTarget) < movement.arriveThreshold)
            {
                movement.stopMovement();
                animator.SetBool("Idle", true);
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
