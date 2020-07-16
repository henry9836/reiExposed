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
        Debug.DrawLine(pos, pos + Vector3.up * 1000.0f, Color.red, 5.0f);

        //Stop movement
        stopMovement();

        //Create a path
        NavMeshPath path = new NavMeshPath();
        bool result = agent.CalculatePath(pos, path);
        if (!result)
        {
            Debug.Log("Invalid Path!");
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

    public void setOverride(OVERRIDE newMode)
    {
        overrideMode = newMode; 
        switch (overrideMode)
        {
            case OVERRIDE.NO_OVERRIDE:
                {
                    Debug.Log("Set No Override");
                    agent.enabled = true;
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
                    Debug.Log("Set All Override");
                    agent.isStopped = true;
                    agent.enabled = false;
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
    }

    private void FixedUpdate()
    {
        if (!goToPosition(lastUpdatedPos))
        {
            stopMovement();
        }
    }

}
