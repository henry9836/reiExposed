using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MythCollisionHandler : AICollisionHandler
{
    Animator animator;
    public float blockStaminaCost = 10.0f;

    public override void Start()
    {
        //Disable AIObjects built-in dectection
        aiObject = GetComponent<AIObject>();
        aiObject.handleCollision = false;

        //Get animator
        animator = GetComponent<Animator>();
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

                        //Block
                        if ((dice <= 5 && aiObject.stamina >= blockStaminaCost) || animator.GetBool("Blocking"))
                        {
                            Debug.Log("Block");
                            aiObject.stamina -= blockStaminaCost;
                            animator.SetTrigger("Block");
                            return;
                        }
                    }
                }

                Debug.Log("No Block");
                animator.SetTrigger("Stun");
                //aiObject.health -= aiObject.playerCtrl.umbreallaDmg;

            }
        }
    }
}
