using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackContainer : MonoBehaviour
{
    public string attackName = "Untitled Attack";
    public float damage = 5.0f;
    public bool damageOnlyOnce = true;
    public AIBody.BodyParts bodyPartsUsedInAttack;
    public bool mustFacePlayer = true;
    public bool overrideTrackingVisonCone = false;
    [Range(0.0f, 1.0f)]
    public float facePlayerThreshold = 0.75f;
    public Vector2 rangeForAttack = new Vector2(1.0f, 10.0f);
    public string triggerName = "Untitled Trigger";

    [Header("AI Modes")]
    public bool allowedModeOne = true;
    public bool allowedModeTwo = true;
    public bool allowedModeThree = true;
    public bool allowedModeFour = true;
    public bool allowedModeFive = true;
    public bool allowedModeSix = true;
    public bool allowedModeSeven = true;
    public bool allowedModeEight = true;
    public bool allowedModeNine = true;
    public bool allowedModeTen = true;

    private AIObject ai;
    private AITracker tracker;

    public bool allowedOnMode(int i)
    {
        switch (i)
        {
            case 1:
                {
                    return allowedModeOne;
                    //break;
                }
            case 2:
                {
                    return allowedModeTwo;
                    //break;
                }
            case 3:
                {
                    return allowedModeThree;
                    //break;
                }
            case 4:
                {
                    return allowedModeFour;
                    //break;
                }
            case 5:
                {
                    return allowedModeFive;
                    //break;
                }
            case 6:
                {
                    return allowedModeSix;
                    //break;
                }
            case 7:
                {
                    return allowedModeSeven;
                    //break;
                }
            case 8:
                {
                    return allowedModeEight;
                    //break;
                }
            case 9:
                {
                    return allowedModeNine;
                    //break;
                }
            case 10:
                {
                    return allowedModeTen;
                    //break;
                }
        }

        Debug.LogWarning($"Cannot check if allowed on mode {i} no logic exists");

        return true;
    }

    public bool queryIfAllowed()
    {
        //////////////////
        return true;
    }

    private void Start()
    {
        ai = GetComponent<AIObject>();
        tracker = GetComponent<AITracker>();
    }


}
