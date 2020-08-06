using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enterToTalk : MonoBehaviour
{

    public GameObject UIelement;
    public bool standing = false;

    public GameObject konbiniUI;
    public GameObject rei;
    public GameObject biginvinstorage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UIelement.gameObject.SetActive(true);
            standing = true;
            GetComponent<AudioSource>().Play();
            biginvinstorage.GetComponent<slot>().itemchange();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            standing = false;
            UIelement.gameObject.SetActive(false);
            GetComponent<AudioSource>().Play();
        }
    }

    void Update()
    {
        if (standing == true)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) // press eneter to acess shop
            {
                ShopNowOpen(true);
            }
        }
    }

    //do stuff when enter / leave
    public void ShopNowOpen(bool isOpen)
    {
        if (isOpen == true)
        {
            konbiniUI.SetActive(true);
            rei.GetComponent<CharacterController>().enabled = false;
            biginvinstorage.transform.root.GetChild(8).gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            biginvinstorage.transform.root.GetComponent<ThePhone>().constantUI.SetActive(false);
        }
        else
        {
            konbiniUI.SetActive(false);
            rei.GetComponent<CharacterController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            biginvinstorage.transform.root.GetComponent<ThePhone>().constantUI.SetActive(true);

        }


    }

   
}
