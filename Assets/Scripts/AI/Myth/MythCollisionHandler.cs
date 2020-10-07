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
    private AudioSource audio;
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
        audio = GetComponent<AudioSource>();

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
            if (!animator.GetBool("Blocking"))
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

                        Debug.Log("No Block");

                        Instantiate(hitVFX, transform.position, Quaternion.identity);

                        animator.SetTrigger("Stun");

                        aiObject.health -= dmg;


                        GameObject tmp = GameObject.Instantiate(this.gameObject.GetComponent<AIObject>().damagedText, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                        tmp.transform.SetParent(this.transform, true);
                        tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + dmg.ToString("F0");
                    }
                }
            }
            //Player hit the enemy during a block
            else
            {
                float dmg = playerCtrl.umbreallaDmg;

                if (playerAnimator.GetBool("HeavyAttack"))
                {
                    dmg = playerCtrl.umbreallaHeavyDmg;
                }

                //Add onto player known attack
                playerUmbrella.targetsTouched.Add(gameObject);

                //If we are facing the player
                if (tracker.isFacingPlayer())
                {
                    //Block
                    playerAnim.SetTrigger("Stun");

                    //Play vfx
                    Instantiate(blockVFX, playerTransform.position, Quaternion.identity);

                    //Play block sfx
                    audio.PlayOneShot(blockSounds[Random.Range(0, blockSounds.Count)]);

                    //Stop blocking
                    animator.ResetTrigger("Block");
                    animator.SetBool("Blocking", false);
                }
                //Take damage
                else
                {
                    if (playerUmbrella.validDmg(gameObject))
                    {
                        Instantiate(hitVFX, transform.position, Quaternion.identity);

                        animator.SetTrigger("Stun");

                        aiObject.health -= dmg;


                        GameObject tmp = GameObject.Instantiate(this.gameObject.GetComponent<AIObject>().damagedText, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                        tmp.transform.SetParent(this.transform, true);
                        tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + dmg.ToString("F0");
                    }
                }
            }
        }
    }
}
