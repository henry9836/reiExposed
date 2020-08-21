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

    public class aiBlobOfThings
    {
        public AIObject ai = null;
        public AITracker tracker = null;
        public Animator animator = null;
        public Transform transform = null;

        public aiBlobOfThings(GameObject obj)
        {
            transform = obj.transform;
            ai = obj.GetComponent<AIObject>();
            tracker = obj.GetComponent<AITracker>();
            animator = obj.GetComponent<Animator>();
        }
    }

    public float informRange = 5.0f;
    public INFORMTYPE type = INFORMTYPE.DISABLED;
    public LayerMask informObjects;

    
    AITracker tracker;
    List<aiBlobOfThings> ais = new List<aiBlobOfThings>();

    public virtual void Start()
    {
        if (type == INFORMTYPE.DISABLED) { return; }

        tracker = GetComponent<AITracker>();

        //Find other objects with our tag
        GameObject[] objs = GameObject.FindGameObjectsWithTag(gameObject.tag);

        //For each object setup our list
        for (int i = 0; i < objs.Length; i++)
        {
            //Do not add outselves
            if (objs[i].name != name)
            {
                ais.Add(new aiBlobOfThings(objs[i]));
            }
        }
    }

    public virtual void Inform()
    {
        switch (type)
        {
            case INFORMTYPE.DISABLED:
                break;
            case INFORMTYPE.RANGE_BASED:
                {
                    //For each ai inform
                    for (int i = 0; i < ais.Count; i++)
                    {
                        if (ais[i].transform != null)
                        {
                            //If the other object cannot see the player
                            if (ais[i].animator.GetBool("CanSeePlayer") == false)
                            {
                                //If the other object is close enough inform
                                if ((transform.position - ais[i].transform.position).sqrMagnitude <= (informRange * informRange))
                                {

                                    Debug.DrawLine(transform.position, ais[i].transform.position, Color.green, 100.0f);

                                    //Inform
                                    ais[i].tracker.lastSeenPos = tracker.lastSeenPos;
                                    ais[i].tracker.lastSeenDir = tracker.lastSeenDir;

                                    //Reset
                                    ais[i].tracker.lostPlayerTimer = 0.0f;
                                    ais[i].tracker.informOverrideTimer = 0.0f;
                                }
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

}
