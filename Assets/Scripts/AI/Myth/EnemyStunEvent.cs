using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunEvent : StateMachineBehaviour
{

    AIBody body;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (body == null)
        {
            body = animator.gameObject.GetComponent<AIBody>();
        }

        body.updateHitBox(AIBody.BodyParts.ALL, false);

        animator.ResetTrigger("Stun");
    }

}
