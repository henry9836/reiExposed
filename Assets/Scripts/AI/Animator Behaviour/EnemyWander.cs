using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

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
            arriveThreshold = ec.arriveDistanceThreshold;
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

        Debug.Log($"{Vector3.Distance(enemy.transform.position, dest)} / {arriveThreshold}");
        Debug.Log($"{enemy.transform.position} :: {dest}");

        //If we are close enough to our wander target stop our AI agent and return to Idle
        if (Vector3.Distance(enemy.transform.position, dest) <= arriveThreshold)
        {
            ec.stopMovement();
            animator.SetBool("Idle", true);
        }
    }

}
