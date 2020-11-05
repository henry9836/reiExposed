using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{

    public int sizeMultiplier = 1;
    public string screenshotName = "screenshot";
    public string fileType = ".png";

    private int screenshotNumber = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ScreenCapture.CaptureScreenshot(screenshotName + screenshotNumber.ToString() + fileType, sizeMultiplier);
            screenshotNumber++;
            Debug.Log("Screenshot Taken!");
        }
    }
}
