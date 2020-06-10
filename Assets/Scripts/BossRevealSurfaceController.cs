using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRevealSurfaceController : MonoBehaviour
{
    public Vector3 outwardDir = Vector3.forward;
    
    private float angleThreshold = 0.1f;

    GameObject player;
    SkinnedMeshRenderer sm;
    Vector3 dirToPlayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sm = GetComponent<SkinnedMeshRenderer>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isPlayerLookingAtMe())
            {
                sm.enabled = true;
                Destroy(this);
            }
        }
    }

    public bool isPlayerLookingAtMe()
    {
        dirToPlayer = (player.transform.position - sm.bounds.center).normalized;
        float dotProd = Vector3.Dot(dirToPlayer, outwardDir);
        return (dotProd > angleThreshold);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (isPlayerLookingAtMe())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(sm.bounds.center, (sm.bounds.center) + (dirToPlayer * 1.0f));
        }
        else
        {
            Gizmos.DrawLine(sm.bounds.center, (sm.bounds.center) + (outwardDir * 1.0f));
        }
    }
}
