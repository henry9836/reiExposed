using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MythMovement : AIMovement
{
    //Ranges for speeds
    public Vector2 moveSpeeds = new Vector2(3.5f, 5.0f);
    public Vector2 rotSpeeds = new Vector2(120.0f, 500.0f);
    public float nearPlayerThreshold = 2.0f;
    public float timeToFullSpeed = 5.0f;

    private float nearPlayerTimer = 0.0f;
    private float accelTimer = 0.0f;
    private Transform playerTransform;

    public override void Start()
    {
        initalPosition = transform.position;
        initalRotSpeed = rotSpeed;
        initalMoveSpeed = moveSpeed;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = rotSpeed;
        animator = GetComponent<AIObject>().animator;
        playerTransform = GetComponent<AIObject>().player.transform;
    }

    public override void FixedUpdate()
    {
        //Get our distance
        if (Vector3.Distance(playerTransform.position, transform.position) < 5.0f)
        {
            nearPlayerTimer += Time.deltaTime;
        }
        else
        {
            nearPlayerTimer = 0.0f;
            accelTimer = 0.0f;
        }

        //Change Speeds based on if we are near the player for too long
        if (nearPlayerTimer > 2.0f)
        {
            accelTimer += Time.deltaTime;
        }

        moveSpeed = Mathf.Lerp(moveSpeeds.x, moveSpeeds.y, (accelTimer / timeToFullSpeed));
        rotSpeed = Mathf.Lerp(rotSpeeds.x, rotSpeeds.y, (accelTimer / timeToFullSpeed));

        //Update agent
        agent.speed = moveSpeed;
        agent.angularSpeed = rotSpeed;

        //If not moving
        //animator.SetBool("Idle", (agent.velocity.magnitude < 1.0f));
    }

}
