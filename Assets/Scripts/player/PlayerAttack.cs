using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : StateMachineBehaviour
{
    public Vector2 attackWindow = new Vector2(0.0f, 0.9f);
    public LayerMask enemyObjectList;
    public bool resetAllOnEnd = false;
    public float movementTime = 0.3f;
    public float movementSpeed = 5.5f;
    [Range(0.0f, 1.0f)]
    public float stopTurning = 0.5f;

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

        //Allow turning during attack
        movementCtrl.canTurnDuringAttack = ((stateInfo.normalizedTime % 1.0f) < stopTurning);

        //Shunt Forwards after we have started our attack
        if (!once && movementTimer < movementTime)
        {
            //Stop player getting on top of the enemy
            //Does the ray intersect any objects in the enemy layers
            RaycastHit hit;
            Vector3 adjustedPos = (characterTrans.position + ((characterTrans.forward * movementSpeed) * 0.5f));
            //if (Physics.BoxCast(adjustedPos, Vector3.one * (movementSpeed * 0.5f), characterTrans.forward, out hit, Quaternion.identity, Mathf.Infinity, enemyObjectList))
            // Cast a sphere wrapping character controller 10 meters forward
            // to see if it is about to hit anything.
            RaycastHit[] hits = Physics.SphereCastAll(adjustedPos, movementSpeed * 0.5f, characterTrans.forward, movementSpeed, enemyObjectList);
            if (hits.Length > 0)
            {

                hit = hits[0];

                //Get correct hit
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hit.distance > hits[i].distance)
                    {
                        hit = hits[i];
                    }
                }
                Debug.Log($"Watch OUT! {hit.point} {hit.collider.name}");
                Debug.DrawLine(hit.point, Vector3.up * 999999.0f, Color.red, 100.0f);
                movementCtrl.forceMovement(characterTrans.forward * (hit.distance - 0.5f));
                //movementCtrl.forceMovement(characterTrans.forward * movementSpeed);
            }
            else
            {
                //Move forwards
                movementCtrl.forceMovement(characterTrans.forward * movementSpeed);
            }
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
