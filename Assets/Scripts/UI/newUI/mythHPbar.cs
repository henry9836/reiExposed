using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mythHPbar : MonoBehaviour
{
    private AIObject aiCtrl;
    private float maxhealth;
    private GameObject player;

    private Image bg;
    private Image foreground;

    private bool showUI = false;

    public float startfadingdist = 30;
    public float finfadingdist = 50;

    void Start()
    {
        aiCtrl = this.transform.root.GetComponent<AIObject>();
        maxhealth = aiCtrl.health;
        bg = this.transform.GetChild(0).gameObject.GetComponent<Image>();
        foreground = this.transform.GetChild(1).gameObject.GetComponent<Image>();
        player = Camera.main.transform.root.gameObject;
    }

    void Update()
    {
        foreground.fillAmount = aiCtrl.health / maxhealth;

        if (aiCtrl.health >= maxhealth)
        {
            showUI = false;
        }
        else
        {
            showUI = true;
        }

        if (showUI == true)
        {
            float dist = (startfadingdist - Vector3.Distance(player.transform.position, this.gameObject.transform.position)) / (finfadingdist - startfadingdist);
            float opacity = Mathf.Clamp(dist, 0.0f, 1.0f);


            foreground.color = new Color(foreground.color.r, foreground.color.g, foreground.color.b, opacity);
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, opacity);
        }
        else
        {
            foreground.color = new Color(foreground.color.r, foreground.color.g, foreground.color.b, 0.0f);
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0.0f);
        }
    }
}
