using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericHitboxController : MonoBehaviour
{

    public float damage = 5.0f;

    public float Damage()
    {
        float tmp = damage;
        //damage = 0.0f;
        GetComponent<Collider>().enabled = false;
        return tmp;
    }
}
