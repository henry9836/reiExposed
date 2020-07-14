using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBossUI : MonoBehaviour
{
    public Image healthBar;
    public Image revealBar;
    public Image lockedUI;
    public Image unlockUI;

    AIObject ai;
    VFXController vfx;
    float initalHealth = 100.0f;
    int initalReveal = 0;

    private void Start()
    {
        ai = GetComponent<AIObject>();
        vfx = GetComponent<VFXController>();
        lockedUI.enabled = true;
        unlockUI.enabled = false;
        initalHealth = ai.startHealth;
    }

    private void FixedUpdate()
    {
        if (initalReveal == 0)
        {
            initalReveal = vfx.bodysNoVFX.Count;
        }

        //Health
        if (ai.health != 0)
        {
            healthBar.fillAmount = ai.health / initalHealth;
        }
        else
        {
            healthBar.fillAmount = 0.0f;
        }

        //Reveal Progress
        if (vfx.bodysNoVFX.Count != 0)
        {
            revealBar.fillAmount = (float)vfx.bodysNoVFX.Count / (float)initalReveal;
        }
        else
        {
            revealBar.fillAmount = 0.0f; 
            lockedUI.enabled = false;
            unlockUI.enabled = true;
        }



    }

}
