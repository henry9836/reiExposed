using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hax : MonoBehaviour
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public List<Image> uiToHide = new List<Image>();
    public List<Text> textToHide = new List<Text>();

    private List<Color> textColor = new List<Color>();
    private List<Color> uiColor = new List<Color>();

    PlayerController pc;
    AIObject boss;
    bool uiHide = false;

    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<AIObject>();

        for (int i = 0; i < uiToHide.Count; i++)
        {
            uiColor.Add(uiToHide[i].color);
        }
        for (int i = 0; i < textToHide.Count; i++)
        {
            textColor.Add(textToHide[i].color);
        }

    }

    private void LateUpdate()
    {
        //Health Hax
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (pc.maxHealth > 100)
            {
                pc.maxHealth = 100.0f;
                pc.health = 100.0f;
            }
            else
            {
                pc.maxHealth = 99999.0f;
                pc.health = 99999.0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            boss.health -= 250.0f;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            SaveSystemController.updateValue("ammoTwo", 4);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            SaveSystemController.updateValue("MythTraces", 5432);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            uiHide = !uiHide;
            if (uiHide == false)
            {
                for (int i = 0; i < uiToHide.Count; i++)
                {
                    uiToHide[i].color = uiColor[i];
                }
                for (int i = 0; i < textToHide.Count; i++)
                {
                    textToHide[i].color = textColor[i];
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            GameObject[] doors = GameObject.FindGameObjectsWithTag("DoorFrame");
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].GetComponent<DoorController>().triggerUnlock();
            }
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            GameObject[] traces = GameObject.FindGameObjectsWithTag("Clue");
            for (int i = 0; i < traces.Length; i++)
            {
                traces[i].GetComponent<TraceController>().Trigger();
            }
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            pc.gameObject.GetComponent<CharacterController>().enabled = false;
            pc.gameObject.transform.position = new Vector3(-1.338f, 41.17f, 25.2f);
            pc.gameObject.GetComponent<CharacterController>().enabled = true;
        }

        if (uiHide)
        {
            for (int i = 0; i < uiToHide.Count; i++)
            {
                uiToHide[i].color = new Color(uiToHide[i].color.r, uiToHide[i].color.g, uiToHide[i].color.b, 0.0f);
            }
            for (int i = 0; i < textToHide.Count; i++)
            {
                textToHide[i].color = new Color(textToHide[i].color.r, textToHide[i].color.g, textToHide[i].color.b, 0.0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            SaveSystemController.saveDataToDisk();
        }

        //Keep hax active
        if (pc.maxHealth > 100)
        {
            pc.health = 99999.0f;
        }
    }
#endif
}
