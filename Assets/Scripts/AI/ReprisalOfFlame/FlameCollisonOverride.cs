using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameCollisonOverride : AICollisionHandler
{

    public FlameAI flameObject;
    private umbrella umbrella;

    // Start is called before the first frame update
    public override void Start()
    {
        //Disable AIObjects built-in dectection
        flameObject = GetComponent<FlameAI>();
        flameObject.handleCollision = false;

        umbrella = flameObject.player.GetComponent<umbrella>();
    }

    //Recieves damage from the player
    public override void OnTriggerEnter(Collider other)
    {
        if (flameObject.health > 0.0f)
        {
            if (other.tag == "PlayerAttackSurface")
            {
                if (umbrella.validDmg(gameObject))
                {
                    umbrella.targetsTouched.Add(gameObject);
                    flameObject.CollisonLogic(other);
                }
            }
        }
    }
}
