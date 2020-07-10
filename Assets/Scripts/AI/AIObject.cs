using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIAttackContainer))]
[RequireComponent(typeof(AIModeSwitcher))]
[RequireComponent(typeof(AITracker))]
[RequireComponent(typeof(AIMovement))]
[RequireComponent(typeof(AIInformer))]
[RequireComponent(typeof(AIBody))]
[RequireComponent(typeof(AIDebugger))]
[RequireComponent(typeof(Animator))]
public class AIObject : MonoBehaviour
{
    public List<AIAttackContainer> attacks = new List<AIAttackContainer>();
    public AIModeSwitcher switcher;
    public AITracker tracker;
    public AIMovement movement;
    public AIInformer informer;
    public AIBody body;

    public float health = 300.0f;
    public AIAttackContainer selectedAttack;
    [Range(1, 10)]
    public int amountofModes = 1;

    [HideInInspector]
    public float startHealth = 0.0f;
    [HideInInspector]
    public float revealAmount = 0.0f;
    [HideInInspector]
    public int currentMode = 1;
    [HideInInspector]
    public Animator animator;

    [SerializeField]
    public GameObject player;

    private List<int> validAttacks = new List<int>();

    //Selects a random attack to use againest the player
    public int selectAttack()
    {
        float distance = Vector3.Distance(tracker.lastSeenPos, transform.position);
        validAttacks.Clear();
        int fallbackAttack = 0;
        float closestAttack = Mathf.Infinity;

        //SELECT FROM RANGE AND MODE

        for (int i = 0; i < attacks.Count; i++)
        {
            //Attack can be used in our behaviour mode
            if (attacks[i].allowedOnMode(currentMode))
            {
                //We are within range for an attack
                if (attacks[i].rangeForAttack.y <= distance)
                {
                    validAttacks.Add(i);
                }
                //record attack if it closer than the last closest attack
                else if (distance - attacks[i].rangeForAttack.y < closestAttack)
                {
                    closestAttack = distance - attacks[i].rangeForAttack.y;
                    fallbackAttack = i;
                }
            }
        }

        //If validAttack is populated
        if (validAttacks.Count > 0)
        {
            bindAttack(validAttacks[Random.Range(0, validAttacks.Count)]);
        }
        //Use fallback attack
        else
        {
            bindAttack(fallbackAttack);
        }

        return 0;
    }

    public void bindAttack(int i)
    {
        if (i < attacks.Count && i >= 0)
        {
            selectedAttack = attacks[i];
        }
    }

    public void unbindAttack()
    {
        selectedAttack = null;
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
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        animator = GetComponent<Animator>();

        //Safety Checks
        selectedAttack = null;
        currentMode = 0;

        //Disable hitboxes
        body.updateHitBox(AIBody.BodyParts.ALL, false);
    }

    public AIAttackContainer getSelectedAttack()
    {
        return selectedAttack;
    }
}