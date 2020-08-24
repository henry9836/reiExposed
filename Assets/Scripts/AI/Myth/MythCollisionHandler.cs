using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MythCollisionHandler : AICollisionHandler
{
    Animator animator;
    Animator playerAnimator;
    public float blockStaminaCost = 10.0f;
    [HideInInspector]
    public bool fullyBlocking = false;

    private AITracker tracker;

    public override void Start()
    {
        //Disable AIObjects built-in dectection
        aiObject = GetComponent<AIObject>();
        aiObject.handleCollision = false;

        playerAnimator = aiObject.player.GetComponent<Animator>();

        //Get Tracker
        tracker = aiObject.tracker;

        //Get animator
        animator = GetComponent<Animator>();

        fullyBlocking = false;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttackSurface")
        {
            if (aiObject.health > 0.0f)
            {
                if (other.tag == "PlayerAttackSurface")
                {
                    //Passive
                    if (aiObject.currentMode == 1)
                    {
                        //Roll for block
                        int dice = Random.Range(0, 10);

                        //If we are facing the player
                        if (tracker.isFacingPlayer())
                        {
                            //Block
                            if ((dice <= 5 && aiObject.stamina >= blockStaminaCost) || animator.GetBool("Blocking"))
                            {

                                if (animator.GetBool("Blocking") && fullyBlocking)
                                {
                                    //Stun the player if they hit an already blocking myth
                                    playerAnimator.SetTrigger("KnockDown");
                                }

                                Debug.Log("Block");
                                aiObject.stamina -= blockStaminaCost;
                                animator.SetTrigger("Block");

                                return;
                            }
                        }
                    }
                }

                Debug.Log("No Block");
                animator.SetTrigger("Stun");
                aiObject.health -= aiObject.playerCtrl.umbreallaDmg;
            }
        }
    }
}
