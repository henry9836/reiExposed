using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpSlamEvent : StateMachineBehaviour
{
    [Range(0.0f, 1.0f)]
    public float jumpFrame = 0.2f;
    public float jumpScale = 10.0f;
    public AnimationCurve jumpPath;

    GameObject enemy;
    EnemyController ec;
    GameObject player;
    Vector3 target;
    Vector3 origin;
    Vector3 progressPosition;
    bool jumped = false;
    float jumpTime = 1.0f;
    float jumpTimer = 0.0f;
    float x = 0.0f;
    float y = 0.0f;
    float t = 0.0f;

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

        jumped = false;
        //Get Jump Time
        jumpTime = stateInfo.length - (stateInfo.length * jumpFrame);
        jumpTimer = 0.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float frameProgress = stateInfo.normalizedTime % 1.0f;

        if (frameProgress >= jumpFrame && !jumped)
        {
            jumped = true;
            //Get a target
            target = player.transform.position;
            origin = enemy.transform.position;
        }

        //Follow Jump Path To Target and follow y * scale
        if (jumped)
        {
            if (jumpTimer <= 1.0f)
            {
                //time
                t = jumpTimer / jumpTime;

                //x
                x = Mathf.Lerp(0.0f, 1.0f, t);

                //y position
                y = jumpPath.Evaluate(x);

                //Scale with scale
                y *= jumpScale;

                //Offset from origin
                y += origin.y;

                //Move along to target
                progressPosition = Vector3.Lerp(origin, target, t);

                //Move up by offset
                progressPosition.y += y;

                //Move to progress position
                enemy.transform.position = progressPosition;

                jumpTimer += Time.deltaTime;
            }
            //At end of jump
            else
            {
                animator.SetBool("Jump", false);
            }
        }


    }
}
