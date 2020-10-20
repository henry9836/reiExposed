using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readyShoot : StateMachineBehaviour
{
    private umbrella umbrella;
    private bool once = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (umbrella == null)
        {
            umbrella = GameObject.FindGameObjectWithTag("Player").GetComponent<umbrella>();
        }

        umbrella.canfire = true;
        umbrella.ISBLockjing = true;


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("Blocking") == false && animator.GetBool("Sprinting") == true && animator.GetBool("Running") == true)
        {
            once = false;
            umbrella.canfire = false;
            umbrella.ISBLockjing = false;
            Debug.Log("sprint no shoot");

        }
        else if (animator.GetBool("Blocking") == false && animator.GetBool("Sprinting") == false && animator.GetBool("Running") == true)
        {
            once = false;
            umbrella.canfire = false;
            umbrella.ISBLockjing = false;
            Debug.Log("run no shoot");

        }
        else if (animator.GetBool("Blocking") == false && animator.GetBool("Running") == false)
        {
            once = false;
            umbrella.canfire = false;
            umbrella.ISBLockjing = false;
            Debug.Log("default no shoot");

        }


    }
}
