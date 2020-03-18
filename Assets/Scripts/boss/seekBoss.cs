using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class seekBoss : StateMachineBehaviour
{

    float needAttackRange = 10.0f;
    BossController bc;
    GameObject player;
    NavMeshAgent agent;
    BossController.bossAttacks attack;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Get Infomation
        if (bc == null)
        {
            bc = animator.gameObject.GetComponent<BossController>();
            player = bc.player;
            agent = bc.agent;
        }
        //Pick a random attack
        attack = (BossController.bossAttacks)Random.Range(0, (int)BossController.bossAttacks.FIREBALL + 1);

        //Query attack range from boss controller
        bc.neededAttackRange = bc.attackTriggerRanges[(int)attack];
        needAttackRange = bc.neededAttackRange;
        bc.animationOverrideFunc(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Move to player and attack when in range
        agent.SetDestination(player.transform.position);

        //If we are close enough to the player stop and attack
        if (Vector3.Distance(player.transform.position, animator.gameObject.transform.position) <= needAttackRange)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.isStopped = false;

            animator.SetTrigger("Slam");

            switch (attack)
            {
                case BossController.bossAttacks.BODYSLAM:
                    {
                        break;
                    }
                case BossController.bossAttacks.CHARGE:
                    {
                        //Aim at player
                        animator.SetBool("Charge", true);
                        break;
                    }
                case BossController.bossAttacks.FIREBALL:
                    {
                        break;
                    }
                case BossController.bossAttacks.MONKEYSLAM:
                    {
                        animator.SetTrigger("Slam");
                        break;
                    }
                case BossController.bossAttacks.SWIPE:
                    {
                        animator.SetTrigger("3Hit");
                        break;
                    }
                default:
                    {
                        Debug.LogWarning($"No trigger found for attack [{attack}]");
                        break;
                    }
            }

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
