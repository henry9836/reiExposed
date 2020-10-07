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
    Animator playerAnim;
    PlayerController playerCtrl;
    public float blockStaminaCost = 10.0f;
    [HideInInspector]
    public bool fullyBlocking = false;

    private AITracker tracker;
    private AudioSource audioSrc;
    private Transform playerTransform;

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
        audioSrc = GetComponent<AudioSource>();

        //Get Umbrella
        playerUmbrella = aiObject.player.GetComponent<umbrella>();
        playerCtrl = aiObject.player.GetComponent<PlayerController>();
        playerAnim = aiObject.player.GetComponent<Animator>();
        playerTransform = aiObject.player.transform;

        fullyBlocking = false;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttackSurface")
        {
            if (aiObject.health > 0.0f)
            {
                //If we can be currently damaged
                if (playerUmbrella.validDmg(gameObject))
                {
                    //If we are currently not blocking or we are blocking but the player is hitting us in the back
                    if (!animator.GetBool("Blocking") || (animator.GetBool("Blocking") && !tracker.isFacingPlayer()))
                    {
                        //Recieve damage and get stunned

                        //Get damage value
                        float dmg = playerCtrl.umbreallaDmg;

                        if (playerAnimator.GetBool("HeavyAttack"))
                        {
                            dmg = playerCtrl.umbreallaHeavyDmg;
                        }

                        //Deal Damage
                        aiObject.health -= dmg;

                        //Add onto player known attack
                        playerUmbrella.targetsTouched.Add(gameObject);

                        //Visible feedback
                        Instantiate(hitVFX, transform.position, Quaternion.identity);
                        animator.SetTrigger("Stun");

                        GameObject tmp = GameObject.Instantiate(this.gameObject.GetComponent<AIObject>().damagedText, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                        tmp.transform.SetParent(this.transform, true);
                        tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + dmg.ToString("F0");
                    }
                    //We are blocking
                    else
                    {
                        //Stun player and go into attack mode
                        //Block
                        playerAnim.SetTrigger("Stun");

                        //Play vfx
                        Instantiate(blockVFX, playerTransform.position, Quaternion.identity);

                        //Play block sfx
                        audioSrc.PlayOneShot(blockSounds[Random.Range(0, blockSounds.Count)]);

                        //Stop blocking
                        animator.ResetTrigger("Block");
                        animator.SetBool("Blocking", false);
                    }
                }
            }
        }
    }
}
