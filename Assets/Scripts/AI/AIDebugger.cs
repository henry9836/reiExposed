using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AIDebugger : MonoBehaviour
{
#if UNITY_EDITOR
    public bool debugMode = false;

    AIBody body;
    AIMovement movement;
    AITracker tracker;
    AIObject ai;

    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            //Checks
            if (body == null)
            {
                body = GetComponent<AIBody>();
            }
            if (ai == null)
            {
                ai = GetComponent<AIObject>();
            }
            if (tracker == null)
            {
                tracker = GetComponent<AITracker>();
            }
            if (movement == null)
            {
                movement = GetComponent<AIMovement>();
            }

            if (Application.isPlaying)
            {

                //Visual Drawing
                Gizmos.color = new Color(1.0f, 0.69f, 0.0f);
                Gizmos.DrawSphere(movement.getDest(), 0.5f);


                //Wander
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(movement.initalPosition, new Vector3(movement.wanderRange * 2.0f, 2.0f, movement.wanderRange * 2.0f));
                //if (!ai.animator.GetBool("CanSeePlayer") && !ai.animator.GetBool("LosingPlayer"))
                //{
                //    Gizmos.color = Color.red;
                //    Gizmos.DrawSphere(movement.lastUpdatedPos, 0.5f);
                //}

                //Targetting
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(tracker.lastSeenPos, 0.5f);
                if (ai.animator.GetBool("LosingPlayer"))
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(tracker.predictedPlayerPos, 0.5f);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(tracker.predictedPlayerPos, new Vector3(tracker.seekWanderRange, 2.0f, tracker.seekWanderRange));

                    Gizmos.color = Color.red;
                    Debug.DrawLine(ai.transform.position, ai.player.transform.position);
                }
                else if (ai.animator.GetBool("CanSeePlayer"))
                {
                    Gizmos.color = Color.green;
                    Debug.DrawLine(ai.transform.position, ai.player.transform.position);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Debug.DrawLine(ai.transform.position, ai.player.transform.position);
                }

                //Body
                Gizmos.color = Color.white;
                for (int i = 0; i < body.head.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.head[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.leftarms.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.leftarms[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.rightarms.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.rightarms[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.leftlegs.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.leftlegs[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.rightlegs.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.rightlegs[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.body.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.body[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.custom1.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.custom1[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.custom2.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.custom2[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.custom3.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.custom3[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.custom4.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.custom4[i].transform.position, 0.2f);
                }
            }
            else
            {
                //Wander
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(transform.position, new Vector3(movement.wanderRange * 2.0f, 2.0f, movement.wanderRange * 2.0f));

                //Body
                Gizmos.color = Color.white;
                for (int i = 0; i < body.head.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.head[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.leftarms.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.leftarms[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.rightarms.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.rightarms[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.leftlegs.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.leftlegs[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.rightlegs.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.rightlegs[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.body.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.body[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.custom1.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.custom1[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.custom2.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.custom2[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.custom3.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.custom3[i].transform.position, 0.2f);
                }
                for (int i = 0; i < body.custom4.Count; i++)
                {
                    Gizmos.DrawWireSphere(body.custom4[i].transform.position, 0.2f);
                }
            }
        }
    }

#endif
}
