using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class ParentTheThingToTheThing : MonoBehaviour
{
    public Transform rootBone;
    public Transform getTransform()
    {
        return rootBone;
    }

}