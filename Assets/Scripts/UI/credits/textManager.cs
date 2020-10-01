using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textManager : MonoBehaviour
{

    void Update()
    {
        this.GetComponent<RectTransform>().localPosition += new Vector3(0.0f, 0.3f, 0.0f);
    }
}
