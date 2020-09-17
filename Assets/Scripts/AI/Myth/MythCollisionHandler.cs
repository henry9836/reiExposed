using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class MythCollisionHandler : AICollisionHandler
{
    public GameObject blockVFX;
    public GameObject hitVFX;
    public List<AudioClip> blockSounds = new List<AudioClip>();

    Animator animator;
    Animator playerAnimator;
    umbrella playerUmbrella;
    PlayerController playerCtrl;
    public float blockStaminaCost = 10.0f;
    [HideInInspector]
    public bool fullyBlocking = false;

    private AITracker tracker;
    private AudioSource audio;

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
        audio = GetComponent<AudioSource>();

        //Get Umbrella
        playerUmbrella = aiObject.player.GetComponent<umbrella>();
        playerCtrl = aiObject.player.GetComponent<PlayerController>();

        fullyBlocking = false;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttackSurface")
        {
            if (aiObject.health > 0.0f)
            {
                if (playerUmbrella.validDmg(gameObject))
                {

                    float dmg = playerCtrl.umbreallaDmg;

                    if (playerAnimator.GetBool("HeavyAttack"))
                    {
                        dmg = playerCtrl.umbreallaHeavyDmg;
                    }

                    //Add onto player known attack
                    playerUmbrella.targetsTouched.Add(gameObject);

                    //Passive
                    if (aiObject.currentMode == 1)
                    {
                        //Roll for block
                        int dice = Random.Range(0, 10);

                        //If we are facing the player
                        if (tracker.isFacingPlayer())
                        {
                            //Block
                            if (((dice <= 8 && aiObject.stamina >= blockStaminaCost) || animator.GetBool("Blocking")))
                            {

                                Instantiate(blockVFX, other.transform.position, Quaternion.identity);
                                audio.PlayOneShot(blockSounds[Random.Range(0, blockSounds.Count)]);

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

                    Debug.Log("No Block");

                    Instantiate(hitVFX, transform.position, Quaternion.identity);

                    animator.SetTrigger("Stun");
                    aiObject.health -= dmg;
                }
            }
        }
    }
}
