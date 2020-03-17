using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photo : MonoBehaviour
{
    public List<GameObject> enemy = new List<GameObject>(); 


    void Update()
    {
        if (Input.GetButtonDown("TakePhoto"))
        {
            Debug.Log("kachrrrrr");

        }
    }
}
