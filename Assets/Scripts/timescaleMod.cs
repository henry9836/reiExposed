using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timescaleMod : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Time.timeScale += 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if ((Time.timeScale - 0.1f) > 0.0f) {
                Time.timeScale -= 0.1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }
}
