using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInformer : MonoBehaviour
{
    public enum INFORMTYPE
    {
        DISABLED,
        RANGE_BASED,
        RANGE_CARRIER
    }

    public float informRange = 5.0f;
    public INFORMTYPE type = INFORMTYPE.RANGE_BASED;
    public LayerMask informObjects;

    AITracker tracker;
    
    public void Inform()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, informRange, transform.forward, Mathf.Infinity, informObjects);
        for (int i = 0; i < hits.Length; i++)
        {
            //If we can get an AITracker
            if (hits[i].collider.gameObject.GetComponent<AITracker>())
            {
                hits[i].collider.gameObject.GetComponent<AITracker>().lastSeenPos = tracker.lastSeenPos;
                hits[i].collider.gameObject.GetComponent<AITracker>().lastSeenDir = tracker.lastSeenDir;
            }
        }
    }

    private void Start()
    {
        tracker = GetComponent<AITracker>();
    }

}
