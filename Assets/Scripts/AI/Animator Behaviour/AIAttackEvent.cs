using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackEvent : StateMachineBehaviour
{
    public Vector2 damageWindow;
    public bool SFX;
    public int SFXIndex = 0;
    public bool SFXRandom;
    public Vector2 SFXRandomRange = new Vector2(0, 1);
    public AudioClip clip;

    MultipleSoundObject soundObj;
    AIForwardAnimator forwarder;
    AIObject ai;
    AIAttackContainer attack;
    AIBody.BodyParts parts;
    bool armed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attacking", true);

        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<AIObject>();
        }
        if (ai == null)
        {
            return;
        }
        if (forwarder == null)
        {
            if (animator.GetBehaviour<AIForwardAnimator>() != null)
            {
                forwarder = animator.GetBehaviour<AIForwardAnimator>();
            }
        }
        if (SFX)
        {
            if (soundObj == null)
            {
                if (ai.GetComponent<MultipleSoundObject>() != null)
                {
                    soundObj = ai.GetComponent<MultipleSoundObject>();
                }
            }
        }

        if (forwarder != null)
        {
            forwarder.SetBool("Attacking", true);
        }

        attack = ai.getSelectedAttack();
        parts = attack.bodyPartsUsedInAttack;

        ai.movement.stopMovement();
        armed = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            return;
        }
        //Turn on triggers
        if (damageWindow.y >= stateInfo.normalizedTime && stateInfo.normalizedTime > damageWindow.x && !armed)
        {
            if (SFX && soundObj != null)
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
            }
            ai.body.updateHitBox(parts, true);
            armed = true;
        }
        //Turn off triggers
        else if (armed && damageWindow.y <= stateInfo.normalizedTime)
        {
            ai.body.updateHitBox(parts, false);
            armed = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (armed)
        {
            ai.body.updateHitBox(AIBody.BodyParts.ALL, false);
        }

        animator.SetBool("Attacking", false);
        if (forwarder != null)
        {
            forwarder.SetBool("Attacking", false);
        }
    }
}
