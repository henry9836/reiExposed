using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MythModeSwitcher : AIModeSwitcher
{
    public enum MYTHMODES
    {
        PASSIVE = 1,
        AGRO = 2,
    }

    public override bool switchMode(int newMode)
    {
        Debug.Log($"Switching to mode {newMode}");

        //Change speed depending on mode
        if (newMode == (int)MYTHMODES.PASSIVE)
        {
            ai.movement.moveSpeed = ai.movement.initalMoveSpeed;
            ai.movement.rotSpeed = ai.movement.initalRotSpeed;
        }
        else if (newMode == (int)MYTHMODES.AGRO)
        {
            ai.movement.moveSpeed = ai.movement.initalMoveSpeed * ai.movement.fastMoveMulti;
            ai.movement.rotSpeed = ai.movement.initalRotSpeed * ai.movement.fastRotMulti;
        }

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
}
