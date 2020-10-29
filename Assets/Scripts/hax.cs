using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hax : MonoBehaviour
{

    public List<Image> uiToHide = new List<Image>();
    public List<Text> textToHide = new List<Text>();

    PlayerController pc;
    AIObject boss;
    bool uiHide = false;

    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<AIObject>();
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
                    uiToHide[i].color = new Color(uiToHide[i].color.r, uiToHide[i].color.g, uiToHide[i].color.b, uiToHide[i].color.a);
                }
                for (int i = 0; i < textToHide.Count; i++)
                {
                    textToHide[i].color = new Color(textToHide[i].color.r, textToHide[i].color.g, textToHide[i].color.b, textToHide[i].color.a);
                }
            }
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
}
