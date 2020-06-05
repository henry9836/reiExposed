using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup : MonoBehaviour
{
    public enum powerupType
    {
        HEALTH,
        STAMINA,
        
    };

    public powerupType type;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        



        if (other.gameObject.tag == "Player")
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();

            if (type == powerupType.HEALTH)
            {
                float healthToAdd = Mathf.Clamp(50.0f, 0.0f, pc.maxHealth - pc.health);

                pc.health += healthToAdd;

            }
            else if (type == powerupType.STAMINA)
            {

            }



            Destroy(this.gameObject);
        }
    }


}
