using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextHybridController : MonoBehaviour
{

    public enum TYPE
    {
        NONE,
        MOUSE,
        AUDIO
    }

    public Slider slider;
    public InputField text;
    public TYPE type;

    float previousSlider;
    float previousText;
    float currSlider;
    float currText;

    bool flagText;
    bool flagSlider;

    public cameraControler CC;

    public void UpdateInfomation()
    {

        //Try to parse text to float :)
        if (float.TryParse(text.text, out currText))
        {
            text.text = Mathf.Clamp(currText, 0.0f, 10.0f).ToString();
        }
        else
        {
            currText = previousText;
            text.text = previousText.ToString();
        }

        currSlider = slider.value;

        //compare to previous loop
        if (previousText != currText) 
        {
            flagText = true;
        }
        else if (previousSlider != currSlider)
        {
            flagSlider = true;
        }

        //Debug.Log($"STATES: {flagText}|{flagSlider} ||| {currText}:{previousText} :: {currSlider}:{previousSlider}");

        previousText = currText;
        previousSlider = currSlider;
    }

    //apply new vals
    private void LateUpdate()
    {

        //if flagged change other
        if (flagSlider)
        {
            //Update text to match slider
            text.text = currSlider.ToString();

        }
        else if (flagText)
        {
            //Update slider to match text
            slider.value = currText;
        }

        switch (type)
        {
            case TYPE.MOUSE:
            {
                CC.mouseSensitivity = AdjusterInfo.calcSlider(slider.value);
                SaveSystemController.updateValue("mouseSensitivity", slider.value);

                break;
            }
            case TYPE.AUDIO:
            {
                AudioListener.volume = AdjusterInfo.calcSlider(slider.value) / 10.0f;
                SaveSystemController.updateValue("volume", slider.value);

                break;
            }
            case TYPE.NONE:
            {
                break;    
            }
            default:
            {
                break;
            }
        }

        //Reset flags
        flagText = false;
        flagSlider = false;
    }


   
}
