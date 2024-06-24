using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureScreenshot : MonoBehaviourService<CaptureScreenshot>
{
    public string screenshotFilename = "Screenshot\\screenshot.png";

    public KeyCode screenshotKey = KeyCode.P;

    void Update()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            ScreenCapture.CaptureScreenshot(screenshotFilename);
            Debug.Log("Successfully take screenshot: " + screenshotFilename);
        }
    }
}
