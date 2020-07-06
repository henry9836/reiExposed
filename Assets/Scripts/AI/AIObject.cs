using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIAttackContainer))]
[RequireComponent(typeof(AIModeSwitcher))]
[RequireComponent(typeof(AITracker))]
[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(AIInformer))]
[RequireComponent(typeof(AIBody))]
public abstract class AIObject : MonoBehaviour
{
    public List<AIAttackContainer> attacks = new List<AIAttackContainer>();
    public AIModeSwitcher switcher;
    public AITracker tracker;
    public AIMovement movement;
    public AIInformer informer;
    public AIBody body;
    public float health = 300.0f;
    [HideInInspector]
    public float startHealth = 0.0f;
    [HideInInspector]
    public float revealAmount = 0.0f;
    [Range(1, 10)]
    public int amountofModes = 1;
    public int selectedAttack = -1;
    [HideInInspector]
    public int currentMode = 0;

    [SerializeField]
    public float movementSpeed = 10.0f;
    [SerializeField]
    public float fastMoveMuilt = 1.5f;
    [SerializeField]
    public float turnSpeed = 2.0f;
    [SerializeField]
    public GameObject player;


    /// <summary>
    /// NEEDS TO BE DONE
    /// </summary>
    /// <returns></returns>
    public int selectAttack()
    {
        //Select best attack from range and allowed modes
        return 0;
    }

    private void Start()
    {
        startHealth = health;
        AIAttackContainer[] attacksArray = GetComponents<AIAttackContainer>();
        for (int i = 0; i < attacksArray.Length; i++)
        {
            attacks.Add(attacksArray[i]);
        }

        //Assign vals if null
        if (switcher == null)
        {
            switcher = GetComponent<AIModeSwitcher>();
        }
        if (tracker == null)
        {
            tracker = GetComponent<AITracker>();
        }
        if (movement == null)
        {
            movement = GetComponent<AIMovement>();
        }
        if (informer == null)
        {
            informer = GetComponent<AIInformer>();
        }
        if (body == null)
        {
            body = GetComponent<AIBody>();
        }

        //Safety Checks
        selectedAttack = -1;
        currentMode = 0;
    }


}