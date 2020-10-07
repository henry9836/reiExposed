using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public bool handleCollision = true;
    public GameObject damagedText;

    public float health = 300.0f;
    public float staminaRegen = 2.5f;
    [Range(0.0f, 1.0f)]
    public float revealThreshold = 0.15f;
    public AIAttackContainer selectedAttack;
    [Range(1, 10)]
    public int amountofModes = 1;

    [HideInInspector]
    public float startHealth = 0.0f;
    
    public float revealAmount = 0.0f;
    [HideInInspector]
    public float stamina;
    //[HideInInspector]
    public int currentMode = 1;
    [HideInInspector]
    public Animator animator;

    [SerializeField]
    public GameObject player;

    [HideInInspector]
    public List<int> validAttacks = new List<int>();
    [HideInInspector]
    public bool deathFlag = false;

    public GameObject revealpersentobject;

    private int lastUsedAttack = -1;

    public virtual void sleepOverride(bool sleep)
    {
        animator.SetBool("Sleeping", sleep);
    }

    //Selects a random attack to use againest the player
    public virtual void selectAttack()
    {
        float distance = Vector3.Distance(tracker.lastSeenPos, transform.position);
        validAttacks.Clear();
        int fallbackAttack = -1;
        float closestAttack = Mathf.Infinity;

        //SELECT FROM RANGE AND MODE
        for (int i = 0; i < attacks.Count; i++)
        {
            //Attack can be used in our behaviour mode
            if (attacks[i].allowedOnMode(currentMode))
            {
                //Debug.Log($"Found Valid Attack {attacks[i].attackName} because it can work in mode |{currentMode}|");
                //We are within range for an attack
                if (attacks[i].rangeForAttack.y >= distance)
                {
                    //If we are not too close for the attack
                    if (attacks[i].rangeForAttack.x <= distance) {
                        //If we have enough stamina for the attack
                        if (attacks[i].statminaNeeded <= stamina)
                        {
                            //If we didn't use this attack before
                            if (i != lastUsedAttack)
                            {
                                validAttacks.Add(i);
                                continue;
                            }
                        }
                    }
                }

                //FALLBACK IF WE COULDN'T ATTACK, FINDS THE BEST POSSIBLE ATTACK ACCORDING TO OUR DISTANCE

                //record attack if it closer than the last closest attack
                if (Mathf.Abs(attacks[i].rangeForAttack.y - distance) < closestAttack)
                {   
                    //If we have enough stamina for the attack
                    if (attacks[i].statminaNeeded <= stamina)
                    {
                        if (i != lastUsedAttack)
                        {
                            closestAttack = Mathf.Abs(attacks[i].rangeForAttack.y - distance);
                            fallbackAttack = i;
                        }
                    }
                }
            }
        }

        //Debug.Log($"last: {lastUsedAttack} Vcount:{validAttacks.Count} F:{fallbackAttack}");

        //unbind last used attack in case of no attack picked
        lastUsedAttack = -1;

        //If validAttack is populated
        if (validAttacks.Count > 0)
        {
            int attack = Random.Range(0, validAttacks.Count);
            lastUsedAttack = attack;
            bindAttack(validAttacks[attack]);
            //Debug.Log($"Valid Used: {attack}");
        }
        //Use fallback attack
        else if (fallbackAttack != -1)
        {
            lastUsedAttack = fallbackAttack;
            bindAttack(fallbackAttack);
            //Debug.Log($"fallbackAttack Used: {fallbackAttack}");
        }
    }

    public virtual AIAttackContainer.EFFECTTYPES QueryDamageEffect()
    {
        if (selectedAttack == null)
        {
            return AIAttackContainer.EFFECTTYPES.NONE;
        }
        else
        {
            return selectedAttack.effect;
        }
    }

    public virtual float QueryDamage()
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

    public virtual void bindAttack(string name)
    {
        for (int i = 0; i < attacks.Count; i++)
        {
            if (attacks[i].attackName == name)
            {
                selectedAttack = attacks[i];
                return;
            }
        }
    }

    public virtual void bindAttack(int i)
    {

        if (i < attacks.Count && i >= 0)
        {
            selectedAttack = attacks[i];
        }
    }

    public virtual void unbindAttack()
    {
        selectedAttack = null;
    }

    public virtual void Awake()
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

        
        sleepOverride(startInSleepState);

    }

    public virtual void FixedUpdate()
    {
        //If we are not sleep
        if (!animator.GetBool("Sleeping"))
        {
            //Increase stamina
            stamina += staminaRegen * Time.deltaTime;
        }


        if (this.gameObject.tag == "Boss")
        {
            revealAmount = revealpersentobject.GetComponent<drawTest>().blackpersent;


            if (revealAmount < revealThreshold)
            {
                Graphics.Blit(revealpersentobject.GetComponent<drawTest>().splatmapColored, revealpersentobject.GetComponent<drawTest>().splatmap);

                revealpersentobject.GetComponent<drawTest>().fromMat.SetTexture("Texture2D_DB299D9F", revealpersentobject.GetComponent<drawTest>().splatmapColored);
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

    public virtual AIAttackContainer getSelectedAttack()
    {
        return selectedAttack;
    }

    //NOT USED ALL COLLISION IS HANDLED IN THEIR OWN COLLISION MANAGERS
    public virtual void OnTriggerEnter(Collider other)
    {
        if (handleCollision)
        {
            if (health > 0.0f)
            {
                if (other.tag == "PlayerAttackSurface")
                {
                    revealAmount = 0.0f;

                    if (this.gameObject.tag == "Boss")
                    {
                        revealAmount = revealpersentobject.GetComponent<drawTest>().blackpersent;
                    }

                    revealAmount = startHealth * revealAmount;

                    float diff = (health - revealAmount);

      

                    if (playerCtrl.umbreallaDmg < diff)
                    {
                        health -= playerCtrl.umbreallaDmg;

                        GameObject tmp = GameObject.Instantiate(damagedText, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                        tmp.transform.SetParent(this.transform, true);
                        tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + playerCtrl.umbreallaDmg.ToString("F0");


                    }
                    else
                    {
                        GameObject tmp = GameObject.Instantiate(damagedText, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                        tmp.transform.SetParent(this.transform, true);
                        tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + diff.ToString("F0");

                        health = revealAmount;

                    }
                }
            }
        }
    }

}