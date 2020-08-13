using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : MonoBehaviour
{
    public enum OVERRIDE{
        NO_OVERRIDE,
        ROT_OVERRIDE,
        MOVE_OVERRIDE,
        FULL_OVERRIDE
    }

    public OVERRIDE overrideMode = OVERRIDE.NO_OVERRIDE;

    public float moveSpeed = 3.5f;
    public float rotSpeed = 120.0f;
    public float fastMoveMulti = 1.5f;
    public float fastRotMulti = 1.5f;
    public float wanderRange = 10.0f;
    public float arriveThreshold = 1.5f;
    [HideInInspector]
    public Vector3 initalPosition = Vector3.zero;
    [HideInInspector]
    public Vector3 lastUpdatedPos;

    private float initalMoveSpeed = 10.0f;
    private float initalRotSpeed = 10.0f;
    private NavMeshAgent agent;
    private Animator animator;

    public Vector3 pickWanderPosition()
    {
        Vector3 target = initalPosition;

        target += new Vector3(Random.Range(-wanderRange, wanderRange), 0.0f, Random.Range(-wanderRange, wanderRange));

        return target;
    }

    public bool agentArrived()
    {
        return (agent.remainingDistance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0.0f);
    }

    public Vector3 getDest()
    {
        return agent.destination;
    }

    //Go to a new position
    public bool goToPosition(Vector3 pos)
    {
        //If we our movement is not overwritten
        if (!agent.isStopped && (overrideMode == OVERRIDE.NO_OVERRIDE || overrideMode == OVERRIDE.ROT_OVERRIDE || overrideMode == OVERRIDE.MOVE_OVERRIDE)) {
            Debug.DrawLine(pos, pos + Vector3.up * 1000.0f, Color.red, 5.0f);

            //Stop movement
            stopMovement();

            //Create a path
            NavMeshPath path = new NavMeshPath();
            bool result = agent.CalculatePath(pos, path);
            if (!result)
            {
                return false;
            }
            else if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
            {
                //Debug.Log("Invalid Path!");
                return false;
            }

            //Go to Destination
            agent.SetPath(path);
            lastUpdatedPos = pos;
            return true;
        }
        return false;
    }

    public void setOverride(OVERRIDE newMode)
    {
        overrideMode = newMode; 
        switch (overrideMode)
        {
            case OVERRIDE.NO_OVERRIDE:
                {
                    agent.isStopped = false;
                    agent.speed = initalMoveSpeed;
                    agent.angularSpeed = initalRotSpeed;
                    break;
                }
            case OVERRIDE.ROT_OVERRIDE:
                {
                    agent.angularSpeed = 0.0f;
                    break;
                }
            case OVERRIDE.MOVE_OVERRIDE:
                {
                    agent.speed = 0.0f;
                    break;
                }
            case OVERRIDE.FULL_OVERRIDE:
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                    break;
                }
            default:
                {
                    Debug.LogWarning($"No behaviour setup for {overrideMode}");
                    break;
                }
        }
    }

    public void stopMovement()
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.isStopped = false;
    }

    private void Start()
    {
        initalPosition = transform.position;
        initalRotSpeed = rotSpeed;
        initalMoveSpeed = moveSpeed;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = rotSpeed;
        animator = GetComponent<AIObject>().animator;
    }

    private void FixedUpdate()
    {
        //If we have a new position
        if (lastUpdatedPos != agent.destination)
        {
            //Move towards our last updatedPos
            if (!goToPosition(lastUpdatedPos))
            {
                //Something went wrong abort
                stopMovement();
            }
        }

        //If not moving
        //animator.SetBool("Idle", (agent.velocity.magnitude < 1.0f));

    }

}
