using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThing : MonoBehaviour
{
    public WeaponThing weapon;

    private void Start()
    {
        Debug.Log("Weapon Damage: " + weapon.damage, this);
    }
}
