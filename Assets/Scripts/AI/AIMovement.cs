using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * 	moveSpeed
	rotSpeed
	movement override
	rotation override
	full_override
	fastMoveSpeedMulti
	fastRotSpeedMulti
	initalRotSpeed
	initalMoveSpeed
	initalPosition
	wanderRange
	canReachDest?
	goToPos
 */

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

    private float initalMoveSpeed = 10.0f;
    private float initalRotSpeed = 10.0f;

    private Vector3 initalPosition = Vector3.zero;
    private NavMeshAgent agent;

    public bool canReachDest(Vector3 dest)
    {
        ///////////////////////

        return true;
    }

    public void goToPosition(Vector3 pos)
    {
        ////////////////////////
    }

    public void setOverride(OVERRIDE newMode)
    {
        overrideMode = newMode;
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
