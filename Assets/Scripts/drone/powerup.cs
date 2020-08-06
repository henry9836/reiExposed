using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup : MonoBehaviour
{
    public enum powerupType
    {
        HEALTH,        
    };

    public powerupType type;

    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Player")
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();

            if (type == powerupType.HEALTH)
            {
                pc.health = pc.maxHealth;
            }

            Destroy(this.gameObject);
        }
    }


}
