﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class MythCollisionHandler : AICollisionHandler
{
    public GameObject blockVFX;
    public GameObject hitVFX;
    public List<AudioClip> hurtSounds = new List<AudioClip>();
    public List<AudioClip> blockSounds = new List<AudioClip>();

    Animator animator;
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

    public void overrideDamage(float dmg)
    {
        //Recieve damage and get stunned
        //Deal Damage
        aiObject.health -= dmg;

        //We now know the player's postion so inform tracker
        tracker.lastSeenPos = playerTransform.position;

        //Visible feedback
        Instantiate(hitVFX, transform.position, Quaternion.identity);
        audioSrc.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Count)]);
        animator.SetTrigger("Stun");

        GameObject tmp = GameObject.Instantiate(this.gameObject.GetComponent<AIObject>().damagedText, transform.position, Quaternion.identity);
        tmp.transform.SetParent(this.transform, true);
        tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + dmg.ToString("F0");
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

                    //If we are currently blocking and we are facing the player
                    if (animator.GetBool("Blocking") && tracker.isFacingPlayer())
                    {
                        //If we were hit by a heavy attack get stunned and slightly damaged
                        if (playerAnim.GetBool("HeavyAttack"))
                        {
                            float dmg = playerCtrl.umbreallaHeavyDmg * 0.25f;

                            //Deal Damage
                            aiObject.health -= dmg;

                            //Add onto player known attack
                            playerUmbrella.targetsTouched.Add(gameObject);

                            //We now know the player's postion so inform tracker
                            tracker.lastSeenPos = playerTransform.position;

                            //Visible feedback
                            Instantiate(hitVFX, transform.position, Quaternion.identity);
                            audioSrc.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Count)]);
                            animator.SetTrigger("Stun");

                            GameObject tmp = GameObject.Instantiate(this.gameObject.GetComponent<AIObject>().damagedText, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                            tmp.transform.SetParent(this.transform, true);
                            tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + dmg.ToString("F0");
                        }
                        //We were not hit by a heavy attack
                        else
                        {
                            //Stun player and go into attack mode
                            //Block
                            playerAnim.SetTrigger("Stun");

                            //Play vfx
                            Instantiate(blockVFX, playerTransform.position, Quaternion.identity);

                            //Play block sfx
                            audioSrc.PlayOneShot(blockSounds[Random.Range(0, blockSounds.Count)]);

                            //maybe we will stop blocking maybe we won't :)
                            //Whack randomly
                            int coin = Random.Range(0, 11);
                            if (coin >= 5)
                            {
                                animator.SetTrigger("Whack");
                                aiObject.bindAttack("Whack");
                            }
                        }
                    }
                    //We are not blocking
                    else
                    {
                        //Recieve damage and get stunned
                        //Get damage value
                        float dmg = playerCtrl.umbreallaDmg;

                        if (playerAnim.GetBool("HeavyAttack"))
                        {
                            dmg = playerCtrl.umbreallaHeavyDmg;
                        }

                        //Deal Damage
                        aiObject.health -= dmg;

                        //Add onto player known attack
                        playerUmbrella.targetsTouched.Add(gameObject);

                        //We now know the player's postion so inform tracker
                        tracker.lastSeenPos = playerTransform.position;

                        //Visible feedback
                        Instantiate(hitVFX, transform.position, Quaternion.identity);
                        audioSrc.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Count)]);
                        animator.SetTrigger("Stun");

                        GameObject tmp = GameObject.Instantiate(this.gameObject.GetComponent<AIObject>().damagedText, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                        tmp.transform.SetParent(this.transform, true);
                        tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + dmg.ToString("F0");
                    }
                }
            }
        }
    }
}
