using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIModeSwitcher : MonoBehaviour
{
    public enum Behaviours
    {
        NO_BEHAVIOUR,
        HEALTH,
        CUSTOM
    }

    public AIModeSwitcher.Behaviours behaviour;

    [HideInInspector]
    public AIObject ai;

    public virtual bool switchMode(int newMode)
    {
        if (newMode <= ai.amountofModes && newMode > 0)
        {
            ai.currentMode = newMode;
        }
        else
        {
            Debug.LogWarning($"Cannot apply new mode {newMode} to {gameObject.name}");
            return false;
        }

        return true;
    }
    public virtual void Start()
    {
        ai = GetComponent<AIObject>();
    }

}
