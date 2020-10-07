using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : StateMachineBehaviour
{

    public bool overrideBoundAttackOnEntry = false;

    AIObject ai;
    AIMovement movement;
    AITracker tracker;
    

    AIAttackContainer attack;
    Transform player;

    float repickAttackThreshold = 5.0f;
    float wrongAttackChosenTimer = 0.0f;

    bool attacked = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<AIObject>();
            movement = ai.movement;
            tracker = ai.tracker;
        }

        player = ai.player.transform;

        //Reset Triggers
        for (int i = 0; i < ai.attacks.Count; i++)
        {
            animator.ResetTrigger(ai.attacks[i].triggerName);
        }

        //If there is no attack bound
        ai.selectAttack();

        attacked = false;
        attack = ai.getSelectedAttack();
        wrongAttackChosenTimer = 0.0f;
        animator.SetBool("Attacking", false);

    }

    Vector3 getBestPositionForAttack()
    {
        Vector3 result = Vector3.zero;
        Vector3 direction = (ai.transform.position - tracker.lastSeenPos).normalized;
        //Go to a position that is in the middle of the range of attack in relation to the player's last seen position
        result = tracker.lastSeenPos + (direction * ((attack.rangeForAttack.y - attack.rangeForAttack.x) * 0.5f));

        return result;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //If we have taken too long to attack
        wrongAttackChosenTimer += Time.deltaTime;
        if (wrongAttackChosenTimer >= repickAttackThreshold)
        {
            movement.stopMovement();
            ai.selectAttack();
            attack = ai.getSelectedAttack();
            wrongAttackChosenTimer = 0.0f;
        }

        //If we can see the player
        if (tracker.canSeePlayer())
        {
            //If we are too far from the player to attack go to player
            if ((Vector3.Distance(ai.transform.position, player.position) > attack.rangeForAttack.y))
            {
                movement.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
                movement.goToPosition(getBestPositionForAttack());
                animator.SetBool("Attacking", false);
            }
            //Too close to attack pick new attack!
            else if (Vector3.Distance(ai.transform.position, player.position) < attack.rangeForAttack.x)
            {
                movement.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
                attacked = false;
                movement.stopMovement();
                ai.selectAttack();
                attack = ai.getSelectedAttack();
                animator.SetBool("Attacking", false);
            }
            //Close enough to attack
            else
            {
                //If there is enough stamina
                if (ai.stamina >= attack.statminaNeeded)
                {
                    //Do we need to face player to attack, if so do we use an override if so 
                    //then compare to override other if we are using an overrride use default 
                    //settings or if we don't care about facing the player
                    if ((attack.mustFacePlayer && tracker.isFacingPlayer() && !attack.overrideTrackingVisionCone) || !attack.mustFacePlayer || (attack.mustFacePlayer && tracker.isFacingPlayer(attack.facePlayerThreshold) && attack.overrideTrackingVisionCone))
                    {
                        //ATTACK

                        //movement.stopMovement();
                        movement.setOverride(AIMovement.OVERRIDE.MOVE_OVERRIDE);

                        if (!attacked)
                        {
                            ai.stamina -= attack.statminaNeeded;
                            animator.SetBool("Attacking", true);
                            attacked = true;
                        }
                        animator.SetTrigger(attack.triggerName);
                    }
                    else
                    {
                        //If we cannot see the player go to the last spot we saw them
                        movement.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
                        movement.goToPosition(tracker.lastSeenPos);
                    }
                }
            }
        }
        //If we cannot see the player go to the last spot we saw them (done in the movement script)
        else
        {
            movement.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
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
