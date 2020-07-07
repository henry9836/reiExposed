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

    public float moveSpeed = 10.0f;
    public float rotSpeed = 10.0f;
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

    public bool canReachDest(Vector3 dest)
    {
        ///////////////////////

        return true;
    }

    public bool agentArrived()
    {
        return (agent.remainingDistance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0.0f);
    }
    public void goToPosition(Vector3 pos)
    {
        agent.SetDestination(pos);
        lastUpdatedPos = pos;
    }

    public void setOverride(OVERRIDE newMode)
    {
        overrideMode = newMode;
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





}
