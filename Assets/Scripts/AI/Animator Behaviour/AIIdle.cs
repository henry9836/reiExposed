using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdle : StateMachineBehaviour
{
    public Vector2 idleWaitTime = new Vector2(0.5f, 5.0f);

    private float timer = 0.0f;
    private float waittime = 0.0f;

    AIObject ai;
    AIMovement movement;

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

        movement.stopMovement();

        waittime = Random.Range(idleWaitTime.x, idleWaitTime.y);
        timer = 0.0f;

        //Reset Values
        animator.ResetTrigger("LostPlayer");

        //Attacks
        for (int i = 0; i < ai.attacks.Count; i++)
        {
            animator.ResetTrigger(ai.attacks[i].triggerName);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer >= waittime)
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
