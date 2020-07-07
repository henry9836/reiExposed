using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITracker : MonoBehaviour
{
    [HideInInspector]
    public Vector3 lastSeenPos;
    [HideInInspector]
    public Vector3 lastSeenDir;
    [HideInInspector]
    public float lastSeenVelo;
    [HideInInspector]
    public Vector3 predictedPlayerPos;

    [SerializeField]
    public Transform target;
    [SerializeField]
    public Transform eyes;
    [Range(0.0f, 1.0f)]
    public float visionCone = 0.75f;
    public float timeTillLostPlayer = 10.0f;

    public LayerMask visionObsctcles;

    private GameObject player;
    private AIObject ai;
    private float lostPlayerTimer = 0.0f;
    private Animator animator;
    

    public Vector3 estimateNewPosition()
    {
        ////////////////////////////////

        return Vector3.zero;
    }

    public bool canSeePlayer() { return canSeePlayer(this.visionCone); }
    public bool canSeePlayer(float _vCone)
    {
        //Raycast check
        ////////////////////////////
        return true;
    }

    public bool isFacingPlayer() { return isFacingPlayer(this.visionCone); }
    public bool isFacingPlayer(float _vCone) {

        ////////////////

        return true;
    }

    private void Start()
    {
        ai = GetComponent<AIObject>();
        player = ai.player;

        if (target == null)
        {
            target = player.transform;
        }
        if (eyes == null)
        {
            eyes = transform;
        }

        animator = ai.animator;
    }

    private void FixedUpdate()
    {
       //Track Player 
    }


}
