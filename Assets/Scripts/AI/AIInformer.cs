using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInformer : MonoBehaviour
{
    /*
     * 	informRange
	informerBool*/

    public enum INFORMTYPE
    {
        DISABLED,
        RANGE_BASED,
        RANGE_CARRIER
    }

    public float informRange = 5.0f;
    public INFORMTYPE type = INFORMTYPE.RANGE_BASED;

    AITracker tracker;
    
    public void Inform()
    {
        //////////////
    }

    private void Start()
    {
        tracker = GetComponent<AITracker>();
    }

}
