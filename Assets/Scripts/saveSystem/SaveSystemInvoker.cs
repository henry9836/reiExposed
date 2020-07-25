using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemInvoker : MonoBehaviour
{
    private void Start()
    {
        //Load this first for things to work
        SaveSystemController.loadDataFromDisk();

        //Get Values
        float one = SaveSystemController.getFloatValue("hahahahah");
        float two = SaveSystemController.getFloatValue("funny funny");
        one += 1.2f;
        float three = one * two;

        //Update Info
        SaveSystemController.updateValue("hahahahah", one);
        SaveSystemController.updateValue("funny funny", two);
        SaveSystemController.updateValue("funny funny123", three);



        //Save
        StartCoroutine(delayed());
    }

    IEnumerator delayed()
    {
        yield return new WaitForSeconds(1.0f);
        SaveSystemController.saveDataToDisk();
        Debug.Log("File written to!");
    }

}
