using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFlameGeyser : StateMachineBehaviour
{

    public int amountOfLoops = 5;
    [Range(0.0f, 1.0f)]
    public float attackTrigger = 0.35f;
    [Range(0.05f, 0.5f)]
    public float minExplodeTime = 0.2f;
    public float allowedTimeOverToAttack = 0.2f;
    public AIBody.BodyParts partsUsed = AIBody.BodyParts.LEFTLEG;
    public GameObject geyserPrefab;

    AIObject ai;
    AITracker tracker;
    Transform player;
    AIBody body;
    AIMovement movement;
    bool attacked = false;
    int amountFired = 0;

    float y = 1.0f;
    float step = 0.25f;

    void SetMath() {
       y = geyserPrefab.GetComponent<GeyserController>().timeTillExplode;
        if (y > amountOfLoops)
        {
            step = amountOfLoops / y;
        }
        else
        {
            step = y / amountOfLoops;
        }
    }

    float GetTime()
    {
        Debug.Log($"{y - (step * amountFired)} Y: {y} step: {step} fired: {amountFired}");
        float t = y - (step * amountFired);
        if (t < minExplodeTime)
        {
            return minExplodeTime;
        }
        return t;
    }

    //Flame Geyser Player
    bool AttackPlayer(Animator animator)
    {
        //Check stamina costs
        if (ai.stamina < ai.getSelectedAttack().statminaNeeded)
        {
            animator.SetBool("Attacking", false);
            return false;
        }
        //Charge boss
        ai.stamina -= ai.getSelectedAttack().statminaNeeded;

        //Logic
        attacked = true;
        amountFired++;
        GameObject gey = Instantiate(geyserPrefab, player.position, Quaternion.identity);
        gey.GetComponent<GeyserController>().timeTillExplode = GetTime();
        return true;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            ai = animator.gameObject.GetComponent<AIObject>();
            if (ai == null)
            {
                return;
            }
        }
        if (tracker == null)
        {
            tracker = ai.tracker;
        }
        if (player == null)
        {
            player = ai.player.transform;
        }
        if (body == null)
        {
            body = ai.body;
        }
        if (movement == null)
        {
            movement = ai.movement;
        }
        attacked = false;
        amountFired = 0;
        SetMath();
        movement.stopMovement();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            return;
        }

        //If in range and have not attacked
        if (!attacked && (((stateInfo.normalizedTime % 1.0f) >= attackTrigger) && ((stateInfo.normalizedTime % 1.0f) < (attackTrigger + allowedTimeOverToAttack))))
        {
            if (!AttackPlayer(animator))
            {
                animator.SetBool("Attacking", false);
                amountFired = amountOfLoops + 5;
            }
        }
        //If out of range 
        else if (((stateInfo.normalizedTime % 1.0f) < attackTrigger) || ((stateInfo.normalizedTime % 1.0f) > (attackTrigger + allowedTimeOverToAttack)))
        {
            attacked = false;
        }

        if (amountFired >= amountOfLoops)
        {
            animator.SetBool("Attacking", false);
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
