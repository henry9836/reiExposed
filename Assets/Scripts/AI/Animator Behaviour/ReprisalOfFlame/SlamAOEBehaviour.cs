using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamAOEBehaviour : StateMachineBehaviour
{

    enum STAGES
    {
        SLAM,
        HIDDEN,
        FLAME,
        FINISHED
    }

    [Range(0.0f, 1.0f)]
    public float disapearTarget = 0.2f;
    [Range(0.0f, 1.0f)]
    public float spawnFlameTarget = 0.5f;
    [Range(0.0f, 1.0f)]
    public float reappearTarget = 0.6f;

    AIObject ai;
    AIMovement movement;
    STAGES currentStage;
    float progress;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ai == null)
        {
            ai = animator.GetComponent<AIObject>();
            movement = ai.movement;
        }


        //Lock into state
        animator.SetBool("Attacking", true);

        currentStage = STAGES.SLAM;

        //Stop movement for attack
        movement.setOverride(AIMovement.OVERRIDE.FULL_OVERRIDE);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        progress = stateInfo.normalizedTime % 1.0f;
        switch (currentStage)
        {
            case STAGES.SLAM:
                {
                    //Disappear
                    if (progress >= disapearTarget)
                    {
                        //Disapear logic

                        currentStage = STAGES.HIDDEN;
                    }
                    Debug.LogWarning("Not Setup");
                    break;
                }
            case STAGES.HIDDEN:
                {
                    if (progress >= spawnFlameTarget)
                    {
                        //Flame visible logic

                        currentStage = STAGES.FLAME;
                    }
                    Debug.LogWarning("Not Setup");
                    break;
                }
            case STAGES.FLAME:
                {
                    if (progress >= reappearTarget)
                    {
                        //Flame visible logic

                        currentStage = STAGES.FINISHED;
                    }
                    Debug.LogWarning("Not Setup");
                    break;
                }
            case STAGES.FINISHED:
                {
                    //Leave state
                    animator.SetBool("Attacking", false);
                    break;
                }
            default:
                {
                    Debug.LogWarning("Unknown Stage Of AOE Attack For Boss " + currentStage.ToString());
                    break;
                }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Resume movement
        movement.setOverride(AIMovement.OVERRIDE.NO_OVERRIDE);
    }

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
