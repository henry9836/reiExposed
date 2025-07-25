﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlameAI : AIObject
{

    private int lastAttackUsed = 0;
    private int amountOfAttacksPickedWhenCloseToPlayer = 0;
    private int amountOfAttacksTillSlam = 4;
    private float closeAttackThreshold = 10.0f;

    private int AOEAttackElement = -1;

    private Animator playerAnim;

    private int findAttack(string attack)
    {
        for (int i = 0; i < attacks.Count; i++)
        {
            if (attacks[i].attackName == attack)
            {
                return i;
            }
        }

        return -1;
    }

    public void overrideDamage(float dmg)
    {

        revealAmount = 0.0f;

        revealAmount = revealpersentobject.GetComponent<drawTest>().blackpersent;

        revealAmount = startHealth * revealAmount;

        float diff = (health - revealAmount);

        if (dmg < diff)
        {
            health -= dmg;
            GameObject tmp = GameObject.Instantiate(damagedText, transform.position, Quaternion.identity);
            tmp.transform.SetParent(this.transform, true);
            tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + dmg.ToString("F0");

        }
        else
        {
            GameObject tmp = GameObject.Instantiate(damagedText, transform.position, Quaternion.identity);
            tmp.transform.SetParent(this.transform, true);
            tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + diff.ToString("F0");

            health = revealAmount;

        }
    }

    //Used by collison handler
    public void CollisonLogic(Collider other)
    {
        if (playerAnim == null)
        {
            playerAnim = other.transform.root.gameObject.GetComponent<Animator>();
        }

        revealAmount = 0.0f;

        revealAmount = revealpersentobject.GetComponent<drawTest>().blackpersent;

        revealAmount = startHealth * revealAmount;

        float diff = (health - revealAmount);

        float dmg;

        if (playerAnim.GetBool("HeavyAttack"))
        {
            dmg = playerCtrl.umbreallaHeavyDmg;
        }
        else
        {
            dmg = playerCtrl.umbreallaDmg;
        }

        if (dmg < diff)
        {
            health -= dmg;
            GameObject tmp = GameObject.Instantiate(damagedText, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
            tmp.transform.SetParent(this.transform, true);
            tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + dmg.ToString("F0");

        }
        else
        {
            GameObject tmp = GameObject.Instantiate(damagedText, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
            tmp.transform.SetParent(this.transform, true);
            tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + diff.ToString("F0");

            health = revealAmount;

        }
    }

    //Selects a random attack to use againest the player
    public override void selectAttack()
    {
        float distance = Vector3.Distance(tracker.lastSeenPos, transform.position);
        validAttacks.Clear();
        int fallbackAttack = 0;
        float closestAttack = Mathf.Infinity;

        if (AOEAttackElement == -1)
        {
            AOEAttackElement = findAttack("AOE");
        }

        if (distance <= closeAttackThreshold)
        {
            amountOfAttacksPickedWhenCloseToPlayer++;
        }
        else
        {
            amountOfAttacksPickedWhenCloseToPlayer = 0;
        }
        
        //If we have attacked the player enough times up close and we have stamina do AOE attack
        if (amountOfAttacksPickedWhenCloseToPlayer >= amountOfAttacksTillSlam && attacks[AOEAttackElement].statminaNeeded <= stamina && attacks[AOEAttackElement].allowedOnMode(currentMode))
        {
            //bind AOE Attack
            bindAttack("AOE");
            amountOfAttacksPickedWhenCloseToPlayer = 0;
            amountOfAttacksTillSlam = Random.Range(4, 7);
            return;
        }

        //SELECT FROM RANGE AND MODE
        for (int i = 0; i < attacks.Count; i++)
        {
            //If this isn't the AOE Attack
            if (attacks[i].attackName != "AOE")
            {
                //Attack can be used in our behaviour mode
                if (attacks[i].allowedOnMode(currentMode))
                {
                    //We are within range for an attack
                    if (attacks[i].rangeForAttack.y >= distance)
                    {
                        //if we are not too close for an attack
                        if (attacks[i].rangeForAttack.x <= distance)
                        {
                            //If we have enough stamina for the attack
                            if (attacks[i].statminaNeeded <= stamina)
                            {
                                //If we haven't just used this attack
                                if (lastAttackUsed != i)
                                {
                                    validAttacks.Add(i);
                                }
                            }
                        }
                    }
                    //record attack if it closer than the last closest attack
                    else if (distance - attacks[i].rangeForAttack.y < closestAttack)
                    {
                        //If we have enough stamina for the attack
                        if (attacks[i].statminaNeeded <= stamina)
                        {
                            closestAttack = distance - attacks[i].rangeForAttack.y;
                            fallbackAttack = i;
                        }
                    }
                }
                
            }
        }

        //If validAttack is populated
        if (validAttacks.Count > 0)
        {
            int element = Random.Range(0, validAttacks.Count);
            lastAttackUsed = element;
            bindAttack(validAttacks[element]);
        }
        //Use fallback attack
        else
        {
            lastAttackUsed = fallbackAttack;
            bindAttack(fallbackAttack);
        }
    }

}
