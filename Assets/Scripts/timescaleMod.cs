using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timescaleMod : MonoBehaviour
{


    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Time.timeScale += 0.1f;   
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Time.timeScale -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale = 1.0f;
        }
    }
}
