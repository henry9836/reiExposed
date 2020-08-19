using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class droneteleport : MonoBehaviour
{
    public GameObject UIelement;
    public bool standing = false;
    public int scene = 2;

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
                SceneToLoadPersistant.sceneToLoadInto = scene;
                SceneManager.LoadScene(1);
            }
        }
    }
}
