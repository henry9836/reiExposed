using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageQuery : MonoBehaviour
{

    public EnemyController ec;

    public float QueryDamage()
    {
        return ec.QueryDamage(); 
    }
}
