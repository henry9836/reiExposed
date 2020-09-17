using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowSlider : MonoBehaviour
{

    public float speed = 5.0f;
    public float timeTillChangeAfterNoChange = 1.0f;
    public Image sliderToFollow;

    private float timer;
    private float lastSeenVal;
    private Image slider;

    private void Start()
    {
        lastSeenVal = sliderToFollow.fillAmount;
        slider = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        //If the value is not equal to previously known value
        if (lastSeenVal != sliderToFollow.fillAmount)
        {
            if (lastSeenVal > sliderToFollow.fillAmount)
            {
                timer = 0.0f;
            }
            lastSeenVal = sliderToFollow.fillAmount;
        }

        //Make the timer go up
        timer += Time.deltaTime;


        //If the value is big then just snap
        if (lastSeenVal > slider.fillAmount)
        {
            slider.fillAmount = lastSeenVal;
        }
        //Move smoothly towards bar after a delay
        else if (lastSeenVal < slider.fillAmount) {
            if (timer > timeTillChangeAfterNoChange)
            {
                if (lastSeenVal != slider.fillAmount)
                {
                    //Move towards value
                    slider.fillAmount -= speed * Time.deltaTime;
                }
            }
        }

    }
}
