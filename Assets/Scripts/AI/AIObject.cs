using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIObject : MonoBehaviour
{
    public List<> attacks;
    public float health = 300.0f;
    public float startHealth = 0.0f;
    public float revealAmount = 0.0f;
    [Range(1, 10)]
    public int amountofModes = 1;

    [SerializeField]
    public float movementSpeed = 10.0f;
    [SerializeField]
    public float fastMoveMuilt = 1.5f;
    [SerializeField]
    public float turnSpeed = 2.0f;
    [SerializeField]
    public Transform target;
    [SerializeField]
    public GameObject player;

    private void Start()
    {
        startHealth = health;
    }

}