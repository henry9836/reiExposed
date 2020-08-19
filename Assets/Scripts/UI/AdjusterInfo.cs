using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AdjusterInfo
{
    public static float calcSlider(float x)
    {
        return (10.0f * ((x / 10) * (x / 10)) / (1 + (1 - (x / 10)) * 1.3f));
    }
}
