using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSubmitPrompt : MonoBehaviour
{

    public GameObject promptCanvas;
    public GameObject submitUI;
    public ThePhone phoneCtrl;
    public Renderer modelRenderer;
    
    bool active = false;

    private void Start()
    {
        promptCanvas.SetActive(false);
        active = false;

        if (phoneCtrl == null)
        {
            phoneCtrl = GameObject.Find("Canvas").GetComponent<ThePhone>();
        }

    }

    //Check for pic taken
    private void FixedUpdate()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(0) && phoneCtrl.camMode)
            {
                //If is visible on screen
                if (modelRenderer.isVisible)
                {
                    //Debug.Log("Taken photo of boss");
                    //Turn submit UI on and turn out component off
                    submitUI.SetActive(true);
                    Cursor.lockState = CursorLockMode.Confined;
                    promptCanvas.SetActive(false);
                    this.enabled = false;
                }
            }
        }
    }

    public void trigger()
    {
        promptCanvas.SetActive(true);
        active = true;
    }
}
