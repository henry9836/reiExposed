using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class flameDash : StateMachineBehaviour
{

    public float dashThresholdTime = 0.3f;
    public float dashTotalTime = 1.0f;

    GameObject player;
    ReprisialOfFlameController rc;
    GameObject boss;
    Vector3 target;
    Vector3 origin;
    NavMeshAgent agent;
    Transform dashChecker;
    float dashTimer = 0.0f;
    bool STOPFLAG = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rc == null)
        {
            rc = animator.gameObject.GetComponent<ReprisialOfFlameController>();
        }
        if (rc==null) { return; }

        if (player == null)
        {
            player = rc.player;
        }

        if (boss == null)
        {
            boss = animator.gameObject;
        }

        if (agent == null)
        {
            agent = rc.agent;
        }
        if (dashChecker == null)
        {
            dashChecker = rc.dashCheck;
        }

        origin = boss.transform.position;
        target = player.transform.position;
        dashTimer = 0.0f;
        STOPFLAG = false;
        rc.stopMovement();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rc == null) { return; }

        //Obsctcle?
        RaycastHit hit;
        if (Physics.Raycast(dashChecker.position, dashChecker.transform.forward, out hit, 1.0f, rc.dashObstacles))
        {
            STOPFLAG = true;
        }

        //Track player until half way
        if (dashTimer < dashTotalTime * 0.75f)
        {
            target = player.transform.position;
        }

        if (dashThresholdTime <= stateInfo.normalizedTime) {
            //Dash quick to player with a simple lerp
            if (!STOPFLAG)
            {
                boss.transform.position = Vector3.Lerp(origin, target, dashTimer / dashTotalTime);
            }

            dashTimer += Time.deltaTime;
        }

        if (dashTimer >= dashTotalTime || STOPFLAG)
        {
            animator.SetBool("Dashing", false);
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
