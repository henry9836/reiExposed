using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlameButt : StateMachineBehaviour
{
    public float timeToTrigger = 0.35f;

    AIObject ai;
    AITracker tracker;
    Transform player;
    AIBody body;
    AIMovement movement;
    AIFlameButtController butt;
    bool attacked = false;

    //Time for the animation to loop in seconds
    float timeToLoop = 1.0f;
    float progress = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initalisation
        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<AIObject>();
            if (ai == null)
            {
                return;
            }
            movement = ai.movement;
        }

        butt = ai.gameObject.GetComponentInChildren<AIFlameButtController>();

        movement.stopMovement();
        movement.setOverride(AIMovement.OVERRIDE.FULL_OVERRIDE);

        timeToLoop = stateInfo.length;
        attacked = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get progress
        progress += Time.unscaledDeltaTime;

        //If we have not attacked and our timer is beyond the threshold
        if (!attacked && (progress >= timeToTrigger))
        {
            butt.flameItUp();
            attacked = true;
        }
        //Reached end of loop reset and prepare for next attack
        else if (attacked && progress >= timeToLoop)
        {
            attacked = false;
            progress = 0.0f;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movement.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
        //Reset selected attack
        //ai.unbindAttack();
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
