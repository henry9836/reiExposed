using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class camMove : MonoBehaviour
{
    public Animator animationref;

    public enum locations
    { 
        ITEM,
        UMBRELLA,
        STORAGE,
    }

    public void move(locations from, locations to)
    {
        if (from == locations.ITEM)
        {
            if (to == locations.STORAGE)
            {
                animationref.SetFloat("dir", -1.0f);
                animationref.SetInteger("current", 0);
            }
            else if (to == locations.UMBRELLA)
            {
                animationref.SetFloat("dir", 1.0f);
                animationref.SetInteger("current", 1);
            }
        }
        else if (from == locations.UMBRELLA)
        {
            if (to == locations.ITEM)
            {
                animationref.SetFloat("dir", -1.0f);
                animationref.SetInteger("current", 1);
            }
            else if (to == locations.STORAGE)
            {
                animationref.SetFloat("dir", 1.0f);
                animationref.SetInteger("current", 2);
            }
        }
        else if (from == locations.STORAGE)
        {
            if (to == locations.ITEM)
            {
                animationref.SetFloat("dir", 1.0f);
                animationref.SetInteger("current", 0);
            }
            else if (to == locations.UMBRELLA)
            {
                animationref.SetFloat("dir", -1.0f);
                animationref.SetInteger("current", 2);
            }
        }


    }
}
