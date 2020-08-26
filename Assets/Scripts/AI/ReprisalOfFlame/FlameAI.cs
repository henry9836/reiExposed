using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAI : AIObject
{

    private AIAttackContainer lastAttackUsed = null;
    private int amountOfAttacksPickedWhenCloseToPlayer = 0;
    private int amountOfAttacksTillSlam = 3;
    private float closeAttackThreshold = 10.0f;

    //Selects a random attack to use againest the player
    public override void selectAttack()
    {
        float distance = Vector3.Distance(tracker.lastSeenPos, transform.position);
        validAttacks.Clear();
        int fallbackAttack = 0;
        float closestAttack = Mathf.Infinity;

        if (distance <= closeAttackThreshold)
        {
            amountOfAttacksPickedWhenCloseToPlayer++;
        }
        
        if (amountOfAttacksPickedWhenCloseToPlayer >= amountOfAttacksTillSlam)
        {
            //bind AOE Attack
            //bindAttack();
        }

        //SELECT FROM RANGE AND MODE
        for (int i = 0; i < attacks.Count; i++)
        {
            //Attack can be used in our behaviour mode
            if (attacks[i].allowedOnMode(currentMode))
            {
                //We are within range for an attack
                if (attacks[i].rangeForAttack.y >= distance)
                {
                    //If we have enough stamina for the attack
                    if (attacks[i].statminaNeeded <= stamina)
                    {
                        validAttacks.Add(i);
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
            lastAttackUsed = attacks[element];
            bindAttack(validAttacks[element]);
        }
        //Use fallback attack
        else
        {
            lastAttackUsed = attacks[fallbackAttack];
            bindAttack(fallbackAttack);
        }
    }
}
