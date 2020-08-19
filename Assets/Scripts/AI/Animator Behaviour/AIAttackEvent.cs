using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackEvent : StateMachineBehaviour
{
    public Vector2 damageWindow;
   
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
            ai.body.updateHitBox(parts, true);
            armed = true;
        }
        //Turn off triggers
        else if (armed && damageWindow.y <= stateInfo.normalizedTime)
        {
            ai.body.updateHitBox(parts, false);
            armed = false;
            animator.SetBool("Attacking", false);
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
