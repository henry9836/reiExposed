using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackEvent : StateMachineBehaviour
{

    public AIBody.BodyParts attackParts;
    public Vector2 damageWindow;

    AIObject ai;
    AIAttackContainer attack;
    bool armed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attacking", true);

        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<AIObject>();
        }

        attack = ai.getSelectedAttack();

        ai.movement.stopMovement();
        armed = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Turn on triggers
        if (damageWindow.y >= stateInfo.normalizedTime && stateInfo.normalizedTime > damageWindow.x && !armed)
        {
            ai.body.updateHitBox(attackParts, true);
            armed = true;
        }
        //Turn off triggers
        else if (armed && damageWindow.y <= stateInfo.normalizedTime)
        {
            ai.body.updateHitBox(attackParts, false);
            armed = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (armed)
        {
            ai.body.updateHitBox(attackParts, false);
        }

        animator.SetBool("Attacking", false);
    }
}
