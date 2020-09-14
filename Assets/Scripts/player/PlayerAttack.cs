using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : StateMachineBehaviour
{
    public Vector2 attackWindow = new Vector2(0.0f, 0.9f);
    public bool resetAllOnEnd = false;
    public float movementTime = 0.3f;
    public float movementSpeed = 5.5f;
    [Range(0.0f, 1.0f)]
    public float stopTurning = 0.5f;
    public float playshake = 0.0f;
    public bool playshakeonce = false;
    public int attackno;

    private umbrella umbrella;
    private bool once = true;
    private float movementTimer = 0.0f;
    private movementController movementCtrl;
    private Transform characterTrans;
    private PlayerController playerControl;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (umbrella == null)
        {
            umbrella = animator.GetComponent<umbrella>();
        }
        animator.SetBool("Attacking", true);
        animator.SetBool("Attack", false);
        animator.ResetTrigger("GoToNextAttack");
        once = true;
        playshakeonce = true;

        movementCtrl = umbrella.GetComponent<movementController>();
        playerControl = umbrella.GetComponent<PlayerController>();
        characterTrans = movementCtrl.charcterModel.transform;
        movementTimer = 0.0f;

        //Charge Player For Attack
        if (playerControl.staminaAmount >= playerControl.staminaToAttack) {
            playerControl.ChangeStamina(-playerControl.staminaToAttack);
        }
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (once == true)
        {
            if ((stateInfo.normalizedTime % 1.0f) >= attackWindow.x)
            {
                umbrella.Hitbox(true);
                once = false;
            }
        }

        if (playshakeonce == true)
        {
            if ((stateInfo.normalizedTime % 1.0f) >= playshake)
            {
                playshakeonce = false;
                Vector3 passTargetPos = Vector3.zero;
                float passOverallSpeed = 0.0f;
                Vector3 passTargetRot = Vector3.zero;
                shakeOperation.lerpModes funcin = shakeOperation.lerpModes.LINEAR;
                shakeOperation.lerpModes funcout = shakeOperation.lerpModes.LINEAR;
                float speedIn = 0.0f;
                float speedOut = 0.0f;


                if (attackno == 0)
                {
                    passTargetPos = new Vector3(-0.05f, -0.1f, 0.0f);
                    passOverallSpeed = 10.0f;
                    passTargetRot = new Vector3(2.0f, -1.0f, 0.0f);
                    funcin = shakeOperation.lerpModes.INEXPO;
                    funcout = shakeOperation.lerpModes.OUTEXPO;
                    speedIn = 1.5f;
                    speedOut = 0.1f;
                }
                else if (attackno == 1)
                {
                    passTargetPos = new Vector3(0.1f, 0.0f, 0.0f);
                    passOverallSpeed = 10.0f;
                    passTargetRot = new Vector3(-0.5f, 2.0f, 0.0f);
                    funcin = shakeOperation.lerpModes.INEXPO;
                    funcout = shakeOperation.lerpModes.OUTEXPO;
                    speedIn = 1.5f;
                    speedOut = 0.1f;
                }
                else if (attackno == 2)
                {
                    passTargetPos = new Vector3(0.0f, 0.0f, 0.1f);
                    passOverallSpeed = 10.0f;
                    passTargetRot = new Vector3(2.0f, 0.0f, 0.0f);
                    funcin = shakeOperation.lerpModes.INEXPO;
                    funcout = shakeOperation.lerpModes.OUTEXPO;
                    speedIn = 1.0f;
                    speedOut = 0.1f;
                }
                else
                {
                    Debug.LogWarning("whack animation was called but no shake effect was played");
                }

                animator.gameObject.transform.root.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<cameraShake>().addOperation(passTargetPos, passTargetRot, passOverallSpeed, funcin, funcout, speedIn, speedOut);

            }
        }

        //Allow turning during attack
        movementCtrl.canTurnDuringAttack = ((stateInfo.normalizedTime % 1.0f) < stopTurning);

        //Shunt Forwards after we have started our attack
        if (!once && movementTimer < movementTime)
        {
            //Move forwards
            movementCtrl.forceMovement(characterTrans.forward * movementSpeed);

            movementTimer += Time.deltaTime;
        }

        if ((stateInfo.normalizedTime % 1.0f) >= attackWindow.y)
        {
            if (!resetAllOnEnd)
            {
                if (animator.GetBool("Attack"))
                {
                    animator.SetTrigger("GoToNextAttack");
                }
                else
                {
                    animator.SetBool("Attacking", false);
                }
            }
            else
            {
                animator.SetBool("Attacking", false);
                animator.ResetTrigger("GoToNextAttack");
            }
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        umbrella.Hitbox(false);
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
