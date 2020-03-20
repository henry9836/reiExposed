using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallAttack : StateMachineBehaviour
{

    public int fireballAmount = 3;
    public Vector2 fireballAmountRange = new Vector2(2, 5);
    [Range (0.2f, 1.0f)]
    public float fireballThrowFrame = 0.5f;
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

        }

        //Create Fireballs
        GameObject fireballMain = Instantiate(fireballPrefab, bc.fireBallCannonLocations[Random.Range(0, bc.fireBallCannonLocations.Count)].position, Quaternion.identity);
        GameObject fireballMain = Instantiate(fireballPrefab, bc.fireBallCannonLocations[Random.Range(0, bc.fireBallCannonLocations.Count)].position, Quaternion.identity);
        GameObject fireballMain = Instantiate(fireballPrefab, bc.fireBallCannonLocations[Random.Range(0, bc.fireBallCannonLocations.Count)].position, Quaternion.identity);

        //Look in front of player
        Vector3 target = bc.predictPlayerPosition(fireball.GetComponent<fireBallController>().travelSpeed, fireball);

        //offset by 1 in y to hit center
        target.y += 1;
        fireball.transform.LookAt(target);

        Debug.DrawLine(fireball.transform.position, target, Color.magenta, 2.0f);

        //Move fireball at player
        fireball.GetComponent<fireBallController>().fullSpeedAheadCaptain();
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
