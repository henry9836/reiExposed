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
    AITracker tmp = null;

    public virtual void Inform()
    {
        switch (type)
        {
            case INFORMTYPE.DISABLED:
                break;
            case INFORMTYPE.RANGE_BASED:
                {
                    RaycastHit[] hits = Physics.SphereCastAll(transform.position, informRange, transform.forward, Mathf.Infinity, informObjects);
                    for (int i = 0; i < hits.Length; i++)
                    {
                        //If we can get an AITracker
                        if (tmp = hits[i].collider.gameObject.GetComponent<AITracker>())
                        {
                            if (tmp.GetComponent<Animator>().GetBool("CanSeePlayer") == false)
                            {

                                //Inform
                                tmp.lastSeenPos = tracker.lastSeenPos;
                                tmp.lastSeenDir = tracker.lastSeenDir;

                                //Reset
                                tmp.lostPlayerTimer = 0.0f;

                                //Animator
                                tmp.GetComponent<Animator>().SetBool("LosingPlayer", true);
                                tmp.GetComponent<Animator>().SetTrigger("Inform");
                            }
                        }
                    }
                    break;
                }
            case INFORMTYPE.RANGE_CARRIER:
                break;
            default:
                break;
        }
        
    }

    public virtual void Start()
    {
        tracker = GetComponent<AITracker>();
    }

}
