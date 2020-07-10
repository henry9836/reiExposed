using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackIdle : StateMachineBehaviour
{
    GameObject enemy;
    EnemyController ec;
    GameObject player;
    Vector3 startingLoc;

    EnemyController.attack currentAttack;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!enemy)
        {
            enemy = animator.gameObject;
        }
        if (ec == null)
        {
            ec = enemy.GetComponent<EnemyController>();
        }
        if (!player)
        {
            player = ec.player;
        }
        if (startingLoc == Vector3.zero)
        {
            startingLoc = ec.startingLoc;
        }

        //Reset all attacks
        for (int i = 0; i < ec.attacks.Count; i++)
        {
            animator.ResetTrigger(ec.attacks[i]);
        }

        //Pick an attack
        if (ec.hasAttack())
        {
            currentAttack = ec.getAttack();
        }
        else
        {
            currentAttack = ec.pickAttack();
        }

        

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //DEBUGGING
#if UNITY_EDITOR
        ec.updateCurrentMode("ATTACK IDLE");
#endif
        //Lost player?
        animator.SetBool("LosingPlayer", !ec.canSeePlayer());

        //Aggro Mode?
        animator.SetBool("AggressiveMode", ec.aggresiveMode);

        //Are we too close to attack?
        if ((player.transform.position - enemy.transform.position).magnitude < currentAttack.range.x)
        {
            //Switch attack
            currentAttack = ec.pickAttack();
        }
        //Are we close enough to attack and facing player
        else if ((((player.transform.position - enemy.transform.position).magnitude > currentAttack.range.x) && ((player.transform.position - enemy.transform.position).magnitude <= currentAttack.range.y)) && (ec.isLookingAtPlayer(0.3f, true)) && (ec.canSeePlayer()))
        {
            //Attack
            ec.stopMovement();
            animator.SetTrigger(currentAttack.name);
        }
        //Chase player until we are close enough
        else
        {
            ec.GoToTargetPos(ec.lastKnownPlayerPosition);
            animator.SetBool("Idle", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
   
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
