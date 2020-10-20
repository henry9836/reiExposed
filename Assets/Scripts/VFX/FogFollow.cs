using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class FogFollow : MonoBehaviour
{
    public bool followThePlayer = false;
    public float startHeight = -5.0f;
    public float startMaxHeight = 60.0f;

    Transform playerTransform;
    VolumeProfile fogProfile;
    float baseDiff = 0.0f;
    float heightDiff = 0.0f;

    private void Start()
    {
        //Get references
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        fogProfile = GetComponent<Volume>().sharedProfile;
        fogProfile.TryGet<Fog>(out var fogSettings);
        fogSettings.baseHeight.value = startHeight;
        fogSettings.maximumHeight.value = startMaxHeight;

        float bH = fogSettings.baseHeight.value;
        float mH = fogSettings.maximumHeight.value;
        float offset = 0.0f;

        //If h is less than 0
        if (bH < 0.0f)
        {
            //Make the offset the positive value of h
            offset = Mathf.Abs(bH);
            //Get difference
            baseDiff = playerTransform.position.y + offset;
        }
        else
        {
            baseDiff = playerTransform.position.y - bH;
        }
        //If h is less than 0
        if (mH < 0.0f)
        {
            //Make the offset the positive value of h
            offset = Mathf.Abs(mH);
            //Get difference
            heightDiff = playerTransform.position.y + offset;
        }
        else
        {
            heightDiff = playerTransform.position.y - mH;
        }


    }

    private void Update()
    {
        //Test
        fogProfile.TryGet<Fog>(out var fogSettings);

        if (followThePlayer)
        {
            //Match the baseHeight where player was at the start
            fogSettings.baseHeight.value = playerTransform.position.y - baseDiff;
            fogSettings.maximumHeight.value = playerTransform.position.y - heightDiff;
        }
        else
        {
            fogSettings.baseHeight.value = 0.0f - baseDiff;
            fogSettings.maximumHeight.value = 0.0f - heightDiff;
        }
    }

}
