using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISFXEvent : StateMachineBehaviour
{
    [Range(0.0f, 1.0f)]
    public float trigger;
    public AudioClip clip;
    public int SFXIndex = 0;
    public bool SFXRandom;
    public Vector2 SFXRandomRange = new Vector2(0, 1);

    MultipleSoundObject soundObj = null;
    bool triggered = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<MultipleSoundObject>() == null)
        {
            return;
        }
        triggered = false;
        soundObj = animator.gameObject.GetComponent<MultipleSoundObject>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (soundObj == null)
        {
            return;
        }

        //Trigger sfx
        if (trigger >= stateInfo.normalizedTime && !triggered)
        {
            if (clip == null)
            {
                if (SFXRandom)
                {
                    SFXIndex = (int)Random.Range(SFXRandomRange.x, SFXRandomRange.y);
                }
                soundObj.Play(SFXIndex);
            }
            else
            {
                soundObj.Play(clip);
            }
            triggered = true;
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
