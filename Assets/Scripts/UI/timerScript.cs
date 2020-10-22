using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerScript : MonoBehaviour
{
    private float timeSpeed;
    public bool activeated;
    void Start()
    {
        timeSpeed = SaveSystemController.getCurrentTime();
    }


    void Update()
    {
        timeSpeed += Time.deltaTime;
        if (activeated)
        {
            this.gameObject.GetComponent<Text>().text = NetworkUtility.convertToTime(timeSpeed);

        }
        else
        {
            this.gameObject.GetComponent<Text>().text = "";
        }
    }
}
