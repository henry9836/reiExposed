using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MythRangedAttack : StateMachineBehaviour
{

    public int amountToSpawn = 3;
    public float timeBetweenSpawns = 0.5f;
    public GameObject projectile;
    public Transform projectileFireLoc = null;

    AITracker tracker;
    AIModeSwitcher behaviour;
    Transform player;
    int spawnCounter = 0;
    float timeBetweenTimer = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (tracker == null)
        {
            tracker = animator.gameObject.GetComponent<AITracker>();
        }
        if (behaviour == null)
        {
            behaviour = animator.gameObject.GetComponent<AIModeSwitcher>();
        }
        if (player == null)
        {
            player = tracker.target;
        }
        if (projectileFireLoc == null)
        {
            projectileFireLoc = tracker.eyes;
        }
        animator.SetBool("Attacking", true);
        spawnCounter = 0;
        timeBetweenTimer = 0.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeBetweenTimer += Time.deltaTime;

        if (tracker.isFacingPlayer())
        {

            if (timeBetweenTimer >= timeBetweenSpawns)
            {
                GameObject tmp = Instantiate(projectile, projectileFireLoc.position, Quaternion.identity);
                tmp.transform.LookAt(player);
                tmp.GetComponent<fireBallController>().behaviour = behaviour;
                timeBetweenTimer = 0.0f;
                spawnCounter++;
            }

            if (spawnCounter >= amountToSpawn)
            {
                animator.SetBool("Attacking", false);
            }
        }
        else
        {
            animator.SetBool("Attacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    ec.clearAttack();
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
