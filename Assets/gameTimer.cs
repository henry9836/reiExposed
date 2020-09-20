using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameTimer : MonoBehaviour
{
    private void Start()
    {
        NetworkUtility.levelTime = 0.0f;
    }

    private void Update()
    {
        NetworkUtility.levelTime += Time.deltaTime;
    }

}
