using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRevealSurfaceController : MonoBehaviour
{
    public Vector3 outwardDir = Vector3.forward;
    public LayerMask obsctules;

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
        //Disable later
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isPlayerLookingAtMe())
            {
                EnableSurface();
            }
        }
    }

    public void EnableSurface()
    {
        sm.enabled = true;
        Destroy(this);
    }

    public bool isPlayerLookingAtMe()
    {
        dirToPlayer = (player.transform.position - sm.bounds.center).normalized;
        float dotProd = Vector3.Dot(dirToPlayer, outwardDir);
        //If we can see the player
        if (dotProd > angleThreshold)
        {
            //If we can raycast to the player
            RaycastHit hit;
            if (Physics.Raycast(sm.bounds.center, dirToPlayer, out hit, Mathf.Infinity, obsctules))
            {
                return (hit.collider.tag == "Player") || (hit.collider.tag == "PlayerTargetNode");
            }
            return true;
        }
        return false;
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
