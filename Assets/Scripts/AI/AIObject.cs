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
    public PlayerController playerCtrl;
    public Animator forwardAnimationsTo;
    public bool startInSleepState = false;

    public float health = 300.0f;
    public float staminaRegen = 2.5f;
    [Range(0.0f, 1.0f)]
    public float revealThreshold = 0.15f;
    public AIAttackContainer selectedAttack;
    [Range(1, 10)]
    public int amountofModes = 1;

    [HideInInspector]
    public float startHealth = 0.0f;
    [HideInInspector]
    public float revealAmount = 0.0f;
    [HideInInspector]
    public float stamina;
    [HideInInspector]
    public int currentMode = 1;
    [HideInInspector]
    public Animator animator;

    [SerializeField]
    public GameObject player;

    public VFXController vfx;

    private float initalVFXObjects;
    private List<int> validAttacks = new List<int>();
    private bool deathFlag = false;

    public void sleepOverride(bool sleep)
    {
        animator.SetBool("Sleeping", sleep);
    }

    //Selects a random attack to use againest the player
    public void selectAttack()
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
                if (attacks[i].rangeForAttack.y >= distance)
                {
                    //If we have enough stamina for the attack
                    if (attacks[i].statminaNeeded <= stamina)
                    {
                        validAttacks.Add(i);
                    }
                }
                //record attack if it closer than the last closest attack
                else if (distance - attacks[i].rangeForAttack.y < closestAttack)
                {   
                    //If we have enough stamina for the attack
                    if (attacks[i].statminaNeeded <= stamina)
                    {
                        closestAttack = distance - attacks[i].rangeForAttack.y;
                        fallbackAttack = i;
                    }
                }
            }
        }

        //If validAttack is populated
        if (validAttacks.Count > 0)
        {
            Debug.Log("Found Valid Attack");
            bindAttack(validAttacks[Random.Range(0, validAttacks.Count)]);
        }
        //Use fallback attack
        else
        {
            bindAttack(fallbackAttack);
        }
    }

    public float QueryDamage()
    {
        if (selectedAttack == null)
        {
            return 0.0f;
        }
        else
        {
            float dmg = selectedAttack.damage;

            if (selectedAttack.damageOnlyOnce)
            {
                unbindAttack();
            }

            return dmg;
        }
    }

    public void bindAttack(int i)
    {

        if (i < attacks.Count && i >= 0)
        {
            selectedAttack = attacks[i];
            Debug.Log($"Bound Attack {attacks[i].triggerName}");
        }
    }

    public void unbindAttack()
    {
        Debug.Log("unbound Attack");
        selectedAttack = null;
    }

    private void Awake()
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
        if (playerCtrl == null)
        {
            playerCtrl = player.GetComponent<PlayerController>();
        }

        animator = GetComponent<Animator>();


        //Safety Checks
        selectedAttack = null;
        currentMode = 1;

        //Disable hitboxes
        body.updateHitBox(AIBody.BodyParts.ALL, false);

        //VFX if boss
        if (GetComponent<VFXController>() != null)
        {
            vfx = GetComponent<VFXController>();
        }

        if (vfx != null)
        {
            initalVFXObjects = vfx.bodysNoVFX.Count;
        }

        
        sleepOverride(startInSleepState);

    }

    private void FixedUpdate()
    {

        stamina += staminaRegen * Time.deltaTime;


        if (initalVFXObjects == 0)
        {
            initalVFXObjects = vfx.bodysNoVFX.Count;
        }

        //Reveal Update
        if (vfx != null)
        {

            revealAmount = 0.0f;

            if (vfx.bodysNoVFX.Count != 0)
            {
                revealAmount = (float)vfx.bodysNoVFX.Count / (float)initalVFXObjects;
            }

            if (revealAmount < revealThreshold)
            {
                

                for (int i = 0; i < vfx.bodysNoVFX.Count; i++)
                {
                    vfx.bodysNoVFX[i].GetComponent<BossRevealSurfaceController>().EnableSurface();
                }
                vfx.bodysNoVFX.Clear();
            }
        }

        //Death
        if (health <= 0.0f)
        {
            if (!deathFlag)
            {
                health = 0.0f;
                deathFlag = true;
                animator.SetTrigger("Death");
                movement.stopMovement();
            }
        }
    }

    public AIAttackContainer getSelectedAttack()
    {
        return selectedAttack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (health > 0.0f)
        {
            if (other.tag == "PlayerAttackSurface")
            {
                float revealAmount = 0.0f;

                if (vfx != null)
                {
                    if (vfx.bodysNoVFX.Count != 0)
                    {
                        revealAmount = (float)vfx.bodysNoVFX.Count / (float)initalVFXObjects;
                    }

                }

                revealAmount *= startHealth;

                if (playerCtrl.umbreallaDmg < (health - revealAmount))
                {
                    health -= playerCtrl.umbreallaDmg;

                }
                else
                {
                    health = revealAmount;

                }

            }
        }
    }

}