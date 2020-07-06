using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBase : MonoBehaviour
{
    public float health = 300.0f;
    [SerializeField]
    public float revealAmount = 0.0f;
    [SerializeField]
    public float movementSpeed = 10.0f;
    [SerializeField]
    public float fastMoveMuilt = 1.5f;
    [SerializeField]
    public float turnSpeed = 2.0f;
    [SerializeField]
    public Transform target;
}