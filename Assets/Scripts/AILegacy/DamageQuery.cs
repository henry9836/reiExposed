using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageQuery : MonoBehaviour
{

    public EnemyController ec;
    public ReprisialOfFlameController rc;

    public float QueryDamage()
    {
        if (ec)
        {
            return ec.QueryDamage();
        }
        else
        {
            return rc.QueryDamage();
        }
    }
}
