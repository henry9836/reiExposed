using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinmaticCamControler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            toggle();
            this.gameObject.GetComponent<Animator>().SetTrigger("e");
            Debug.Log("playing animation");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            toggle();
            this.gameObject.GetComponent<Animator>().SetTrigger("calmcluet");
            Debug.Log("playing animation");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            toggle();
            this.gameObject.GetComponent<Animator>().SetTrigger("calmdoort");
            Debug.Log("playing animation");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            toggle();
            this.gameObject.GetComponent<Animator>().SetTrigger("calmdoorinvtertedt");
            Debug.Log("playing animation");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            toggle();
            this.gameObject.GetComponent<Animator>().SetTrigger("calmqrt");
            Debug.Log("playing animation");
        }
    }


    private void toggle()
    {
         GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
        this.GetComponent<Camera>().enabled = true;
    }
}
