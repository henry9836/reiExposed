using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWander : StateMachineBehaviour
{

    GameObject enemy;
    EnemyController ec;
    Vector3 dest;

    float arriveThreshold = 0.0f;

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
        if (arriveThreshold == 0.0f)
        {
            //Squared for faster exec
            arriveThreshold = ec.arriveDistanceThreshold * ec.arriveDistanceThreshold;
        }

        dest = ec.wanderTarget;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //DEBUGGING
#if UNITY_EDITOR
        ec.updateCurrentMode("WANDERING");
#endif
        //If we can see the player it time to attack
        if (ec.canSeePlayer())
        {
            ec.stopMovement();
            animator.SetBool("AttackMode", true);
        }

        //If we are close enough to our wander target stop our AI agent and return to Idle
        if ((ec.wanderTarget - enemy.transform.position).sqrMagnitude <= arriveThreshold)
        {
            animator.SetBool("Idle", true);
            ec.stopMovement();
        }
    }

}