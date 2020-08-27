using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAI : AIObject
{

    private int lastAttackUsed = 0;
    private int amountOfAttacksPickedWhenCloseToPlayer = 0;
    private int amountOfAttacksTillSlam = 3;
    private float closeAttackThreshold = 10.0f;

    //Selects a random attack to use againest the player
    public override void selectAttack()
    {
        Debug.Log("[DEBUG] Select Attack Called");
        float distance = Vector3.Distance(tracker.lastSeenPos, transform.position);
        validAttacks.Clear();
        int fallbackAttack = 0;
        float closestAttack = Mathf.Infinity;

        if (distance <= closeAttackThreshold)
        {
            amountOfAttacksPickedWhenCloseToPlayer++;
        }
        else
        {
            amountOfAttacksPickedWhenCloseToPlayer = 0;
        }

        Debug.Log("Amount of attacks close to player: " + amountOfAttacksPickedWhenCloseToPlayer.ToString());
        
        if (amountOfAttacksPickedWhenCloseToPlayer >= amountOfAttacksTillSlam)
        {
            //bind AOE Attack
            bindAttack("AOE");
            amountOfAttacksPickedWhenCloseToPlayer = 0;
            return;
        }

        //SELECT FROM RANGE AND MODE
        for (int i = 0; i < attacks.Count; i++)
        {
            //If this isn't the AOE Attack
            if (attacks[i].attackName != "AOE")
            {
                Debug.Log("Name of attack is " + attacks[i].attackName);

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
