using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enterToTalk : MonoBehaviour
{

    public GameObject UIelement;
    public bool standing = false;

    public GameObject konbiniUI;
    public GameObject rei;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UIelement.gameObject.SetActive(true);
            standing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            standing = false;
            UIelement.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (standing == true)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ShopNowOpen(true);
            }
        }
    }

    public void ShopNowOpen(bool isOpen)
    {
        if (isOpen == true)
        {
            konbiniUI.SetActive(true);
            rei.GetComponent<CharacterController>().enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            konbiniUI.SetActive(false);
            rei.GetComponent<CharacterController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


    }

   
}
