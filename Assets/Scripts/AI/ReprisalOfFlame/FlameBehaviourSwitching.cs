using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBehaviourSwitching : AIModeSwitcher
{
    [Range(0.0f, 1.0f)]
    public List<float> stageThresholds = new List<float>();

    private int currentBehaviour = 0;
    private float maxHealth = 1;

    public override void Start()
    {
        ai = GetComponent<AIObject>();

        currentBehaviour = ai.currentMode;
        maxHealth = ai.startHealth;
    }


    private void FixedUpdate()
    {
        //If next stage exists
        if (stageThresholds.Count > currentBehaviour)
        {
            //If we have hit the threshold
            if (stageThresholds[currentBehaviour] >= (ai.health / maxHealth))
            {
                //Switch to the next stage
                currentBehaviour++;
                ai.currentMode = currentBehaviour;
            }
        }

        Debug.Log("Behaviour: " + currentBehaviour.ToString() + " | " + stageThresholds[currentBehaviour].ToString() + " >= " + (ai.health / maxHealth).ToString());

    }
}
