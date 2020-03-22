using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallAttack : StateMachineBehaviour
{

    public int fireballAmount = 3;
    public Vector2 fireballAmountRange = new Vector2(2, 5);
    [Range(0.2f, 1.0f)]
    public float fireBallSpread = 15.0f;
    public float fireballThrowFrame = 0.5f;
    public Vector2 sizeRange = new Vector2(0.3f, 1.0f);
    public GameObject fireballPrefab;


    BossController bc;
    GameObject player;
    int loopCount = 0;
    bool fireballFired = false;

    void FireFireBall()
    {

        //Pick random amount to throw
        int amountToFire = (int)Random.Range(fireballAmountRange.x, fireballAmountRange.y + 1);

        for (int i = 0; i < amountToFire; i++)
        {
            //Create a fireball
            GameObject fireball = Instantiate(fireballPrefab, bc.fireBallCannonLocations[Random.Range(0, bc.fireBallCannonLocations.Count)].position, Quaternion.identity);

            //Go in the direction of the player offset randomly by the spread
            float mySpread = Random.Range(-fireBallSpread, fireBallSpread);

            //Randomise Scale
            float size = Random.Range(sizeRange.x, sizeRange.y);
            fireball.transform.localScale = new Vector3(size, size, size);

            Vector3 targetPosition = player.transform.position;

            targetPosition = targetPosition + (Random.insideUnitSphere * mySpread);

            fireball.transform.LookAt(targetPosition);

            Debug.DrawLine(fireball.transform.position, targetPosition, Color.magenta, 2.0f);

            fireball.GetComponent<fireBallController>().fullSpeedAheadCaptain();
        }

    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bc = animator.gameObject.GetComponent<BossController>();
        player = bc.player;
        loopCount = 0;
        bc.animationOverrideFunc(true);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (loopCount > fireballAmount)
        {
            animator.SetTrigger("Exit");
        }
        else
        {
            if (stateInfo.normalizedTime % 1.0f >= fireballThrowFrame && !fireballFired)
            {
                //Fire fireball
                FireFireBall();
                loopCount++;
                fireballFired = true;
                //Check if we are still facing player if we aren't then cancel attack
                if (!bc.isBossLookingAtPlayer(0.75f))
                {
                    animator.SetTrigger("Exit");
                }

            }
            //On animation loop reset
            else if (stateInfo.normalizedTime%1.0f < 0.1f)
            {
                fireballFired = false;
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
