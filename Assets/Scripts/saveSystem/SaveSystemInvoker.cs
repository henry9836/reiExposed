using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemInvoker : MonoBehaviour
{
    private void Awake()
    {
        //Load this first for things to work
        SaveSystemController.loadDataFromDisk();

        /*
         * 
         * EXAMPLES BELOW ON HOW TO USE
         * 
         */

        return;

        //Get Values
        float one = SaveSystemController.getFloatValue("hahahahah");
        float two = SaveSystemController.getFloatValue("funny funny");
        one += 1.2f;
        float three = one * two;
        bool b = SaveSystemController.getBoolValue("Bool");
        b = !b;
        int i = SaveSystemController.getIntValue("Int");
        i++;

        //Update Info
        SaveSystemController.updateValue("hahahahah", one);
        SaveSystemController.updateValue("funny funny", two);
        SaveSystemController.updateValue("funny funny123", three);
        SaveSystemController.updateValue("Bool", b);
        SaveSystemController.updateValue("Int", i);

        //Save
        StartCoroutine(delayed());
    }

    //EXAMPLE ON HOW TO SAVE

    IEnumerator delayed()
    {
        yield return new WaitForSeconds(1.0f);
        //SaveSystemController.saveDataToDisk();
        //Debug.Log("File written to!");
    }

}
