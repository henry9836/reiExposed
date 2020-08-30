using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISFXEvent : StateMachineBehaviour
{
    [Range(0.0f, 1.0f)]
    public float trigger;
    public List<AudioClip> clips = new List<AudioClip>();

    AudioSource audioSrc = null;
    bool triggered = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioSrc == null)
        {
            audioSrc = animator.gameObject.GetComponent<AudioSource>();
            return;
        }
        triggered = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Trigger sfx
        if (trigger <= (stateInfo.normalizedTime % 1.0f) && !triggered)
        {
            audioSrc.PlayOneShot(clips[Random.Range(0, clips.Count)]);
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
