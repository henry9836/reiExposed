using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundQue : StateMachineBehaviour
{

    private AudioSource audio;
    private bool played = false;

    public AudioClip audioClip;
    [Range(0.0f, 1.0f)]
    public float trigger = 0.5f;


    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audio = animator.gameObject.GetComponent<AudioSource>();
        played = false;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float frameProgress = stateInfo.normalizedTime % 1.0f;

        if (!played && (frameProgress >= trigger))
        {
            audio.PlayOneShot(audioClip);
            played = true;
        }
    }
}
