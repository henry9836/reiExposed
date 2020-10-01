using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class gammaAdjust : MonoBehaviour
{
    public Volume post;

    public void setGamma()
    {
        Debug.Log("Set Gamma");
        SaveSystemController.updateValue("Gamma", this.GetComponent<Slider>().value);
        SaveSystemController.saveDataToDisk();

        SceneToLoadPersistant.sceneToLoadInto = 2;
        SceneManager.LoadScene(1);
        Debug.Log("Loading");
    }

    public void updateValue()
    {        
        LiftGammaGain tmp;
        if (post.profile.TryGet(out tmp))
        { 
            tmp.gamma.value = new Vector4(0.0f, 0.0f, 0.0f, this.GetComponent<Slider>().value);
        }
    }
}
