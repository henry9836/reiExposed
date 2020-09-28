using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class gammaAdjust : MonoBehaviour
{
    public Volume post;
    public void updateValue()
    {        
        LiftGammaGain tmp;
        if (post.profile.TryGet(out tmp))
        { 
            tmp.gamma.value = new Vector4(0.0f, 0.0f, 0.0f, this.GetComponent<Slider>().value);
        }
    }
}
