using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVFXEvent : StateMachineBehaviour
{
    [Range(0.0f, 1.0f)]
    public float trigger;
    public int VFXIndex = 0;
    public bool VFXRandom;
    public Vector2 VFXRandomRange = new Vector2(0, 1);

    MultipleVFXHandler vfxObj = null;
    bool triggered = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<MultipleVFXHandler>() == null)
        {
            return;
        }
        triggered = false;
        vfxObj = animator.gameObject.GetComponent<MultipleVFXHandler>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (vfxObj == null)
        {
            return;
        }

        //Trigger sfx
        if (trigger >= stateInfo.normalizedTime && !triggered)
        {
            if (VFXRandom)
            {
                VFXIndex = (int)Random.Range(VFXRandomRange.x, VFXRandomRange.y);
            }
            vfxObj.Play(VFXIndex);
            triggered = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (vfxObj != null)
        {
            vfxObj.StopAll();
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
