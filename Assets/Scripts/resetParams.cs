using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetParams : StateMachineBehaviour
{

    public bool clearOnStart = true;
    public bool clearOnEnd = false;

    public List<string> resetBools = new List<string>();
    public List<string> resetTriggers = new List<string>();



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (clearOnStart)
        {
            for (int i = 0; i < resetBools.Count; i++)
            {
                animator.SetBool(resetBools[i], false);
            }

            for (int i = 0; i < resetTriggers.Count; i++)
            {
                animator.ResetTrigger(resetTriggers[i]);
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (clearOnEnd)
        {
            for (int i = 0; i < resetBools.Count; i++)
            {
                animator.SetBool(resetBools[i], false);
            }

            for (int i = 0; i < resetTriggers.Count; i++)
            {
                animator.ResetTrigger(resetTriggers[i]);
            }
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
