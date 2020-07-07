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
    public Vector3 predictedPlayerPos;

    [SerializeField]
    public Transform target;
    [SerializeField]
    public Transform eyes;
    [Range(0.0f, 1.0f)]
    public float visionCone = 0.75f;
    public float timeTillLostPlayer = 10.0f;
    public float maxSpotDistance = 50.0f;
    public float sixthSenseMaxDistance = 4.0f;

    public LayerMask visionObsctcles;

    private GameObject player;
    private Transform playerTargetNode;
    private Transform playerModel;
    private AIObject ai;
    private float lostPlayerTimer = 0.0f;
    private Animator animator;
    

    public Vector3 estimateNewPosition()
    {
        ////////////////////////////////

        return Vector3.zero;
    }

    public bool canSeePlayer() { return canSeePlayer(visionCone); }
    public bool canSeePlayer(float _vCone)
    {
        //Sixth Sense
        if (Vector3.Distance(transform.position, player.transform.position) <= sixthSenseMaxDistance)
        {
            return true;
        }
        //Raycast check
        else 
        { 
            RaycastHit hit;
            if (Physics.Raycast(eyes.position, (playerTargetNode.position - eyes.position).normalized, out hit, maxSpotDistance, visionObsctcles))
            {
                Debug.DrawLine(eyes.position, hit.point, Color.red);
                if (hit.collider.tag == "PlayerTargetNode")
                {
                    //Can we see player within our cone
                    return isFacingPlayer();
                }
            }
        }

        return false;
    }

    public bool isFacingPlayer() { return isFacingPlayer(visionCone); }
    public bool isFacingPlayer(float _vCone) {
        Vector3 dir = (playerTargetNode.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dir, eyes.transform.forward);
        return (dotProd >= _vCone);
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

        playerTargetNode = GameObject.FindGameObjectWithTag("PlayerTargetNode").transform;

        playerModel = player.GetComponent<movementController>().charcterModel.transform;
    }

    private void FixedUpdate()
    {
        //Track Player 
        animator.SetBool("CanSeePlayer", canSeePlayer());

        //Can We See Player
        if (animator.GetBool("CanSeePlayer"))
        {
            //Update Infomation About Player
            lastSeenPos = player.transform.position;
            lastSeenDir = playerModel.forward;

            //Reset
            lostPlayerTimer = 0.0f;
            animator.SetBool("LosingPlayer", false);
        }
        //Losing Player
        else if (lostPlayerTimer <= timeTillLostPlayer)
        {
            lostPlayerTimer += Time.deltaTime;
            animator.SetBool("LosingPlayer", true);

            if (lostPlayerTimer >= timeTillLostPlayer)
            {
                animator.SetTrigger("LostPlayer");
            }

        }

    }


}
