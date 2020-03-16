using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTest : MonoBehaviour
{
    public bool refreshTarget;
    public Transform target;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (refreshTarget)
        {
            agent.SetDestination(target.position);
            refreshTarget = false;
        }
    }
}
