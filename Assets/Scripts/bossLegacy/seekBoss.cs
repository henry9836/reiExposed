using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class seekBoss : StateMachineBehaviour
{

    public float attackAimTimeout = 3.0f;

    float needAttackRange = 10.0f;
    float attackAimTimeoutTimer = 0.0f;
    BossController bc;
    GameObject player;
    NavMeshAgent agent;
    BossController.bossAttacks attack;
    public AudioClip groundSlam;

    void Reset(Animator animator)
    {
        animator.SetBool("Charging", false);
        animator.ResetTrigger("Slam");
        animator.ResetTrigger("3Hit");
        animator.ResetTrigger("Fireball");
        animator.ResetTrigger("BodySlam");
        animator.ResetTrigger("Exit");

        if (animator.GetBool("PlayGroundSlamWhenBack"))
        {
            bc.GetComponent<AudioSource>().PlayOneShot(groundSlam);
        }

        animator.SetBool("PlayGroundSlamWhenBack", false);

    }

    void PickAttack()
    {
        //Filter attacks from distances
        List<BossController.bossAttacks> validAttacks = new List<BossController.bossAttacks>();

        float currentDistance = Vector3.Distance(bc.gameObject.transform.position, player.transform.position);

        for (int i = 0; i < (int)BossController.bossAttacks.FIREBALL; i++)
        {
            //if we are close enough push onto list but not too close
            if ((bc.attackTriggerRanges[i].x < currentDistance) && (bc.attackTriggerRanges[i].y >= currentDistance))
            {
                validAttacks.Add((BossController.bossAttacks)i);
            }
        }

        //If we have a valid attack
        if (validAttacks.Count > 0) 
        {

            //From the valid attacks pick a random attack
            attack = validAttacks[Random.Range(0, validAttacks.Count)];

        }

        //Fallback we don't have an attack
        else
        {
            //Pick a random attack
            attack = (BossController.bossAttacks)Random.Range(0, (int)BossController.bossAttacks.FIREBALL + 1);
        }

        //Query needed attack range from boss controller
        bc.neededAttackRange = bc.attackTriggerRanges[(int)attack].y;
        needAttackRange = bc.neededAttackRange;
        bc.animationOverrideFunc(false);

        attackAimTimeoutTimer = 0.0f;
    }

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


        Reset(animator);
        PickAttack();

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

            //if (bc.isBossLookingAtPlayer(bc.angleThresholdBeforeMoving))
            //{
            //    animator.SetBool("Charging", true);
            //}

            switch (attack)
            {
                case BossController.bossAttacks.BODYSLAM:
                    {
                        //Are we looking at player?
                        if (bc.isBossLookingAtPlayer(bc.angleThresholdBeforeMoving))
                        {
                            animator.SetTrigger("BodySlam");
                        }
                        break;
                    }
                case BossController.bossAttacks.CHARGE:
                    {
                        //Are we looking at player?
                        if (bc.isBossLookingAtPlayer(bc.angleThresholdBeforeMoving))
                        {
                            animator.SetBool("Charging", true);
                        }
                        break;
                    }
                case BossController.bossAttacks.FIREBALL:
                    {
                        if (bc.isBossLookingAtPlayer(bc.angleThresholdBeforeMoving))
                        {
                            animator.SetTrigger("Fireball");
                        }
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

        attackAimTimeoutTimer += Time.deltaTime;

        if (attackAimTimeoutTimer > attackAimTimeout)
        {
            PickAttack();
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
