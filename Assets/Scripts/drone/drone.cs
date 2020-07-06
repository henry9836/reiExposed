using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drone : MonoBehaviour
{
    public List<GameObject> potions;

  
    public void drop(int i)
    {
        GameObject.Instantiate(potions[i], this.gameObject.transform.position, Quaternion.identity);
    }
}
