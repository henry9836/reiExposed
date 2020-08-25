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

    public void move(locations to)
    {
        animationref.SetInteger("current", (int)to);
    }
}
