using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class umbrella : MonoBehaviour
{
    [Header("General")]
    public bool canfire = false;
    public float blockingStamina;
    public float timeTillHeavyAttack = 1.0f;
    public bool cooldown = false;
    public float cooldowntime = 2.0f;
    public float cooldowntimer = 0.0f;
    public LayerMask enemy;
    public LayerMask ball;
    public GameObject damagedText;
    public bool ISBLockjing = false;
    public List<AudioClip> swishSounds = new List<AudioClip>();
    public AudioClip umbrellaActivateSFX;
    public AudioClip umbrellaShoot;
    public BoxCollider umbrellaHitBox;
    public GameObject umbeaalBone;
    public GameObject boss;
    public AudioSource audio;
    public GameObject shotUI;
    public MultipleVFXHandler aimVFX;
    public ParticleSystem shootVFX;
    public bool phoneLock = false;
    [HideInInspector]
    public List<GameObject> targetsTouched = new List<GameObject>();
    public AudioClip noammoClip;

    //shotty
    [Header("Shotty")]
    [HideInInspector]
    public float bulletSpread; // do not touch
    public float bulletSpreadRunning = 0.165f;
    public float bulletSpreadADS = 0.08f;
    public float MaxRange;
    public float MaxDamage;
    public int pellets;
    public GameObject xinsButthole;
    public GameObject crosshair;
    public List<GameObject> xinsButtholes;
    public int buttholeToMove;
    public int ammo;
    public int ammoTwo;
    private int ammocycle;


    private movementController movement;
    [HideInInspector]
    public PlayerController playercontrol;
    private Transform charModel;
    private GameObject cam;
    private Animator animator;
    private bool latetest = false;
    private bool attackQueued = false;
    private bool lastAttackHeavy = false;
    private float timerToHeavy = 0.0f;
    [HideInInspector]
    public float phoneTimer = 0.0f;
    [HideInInspector]
    public float phoneThreshold = 0.25f;
    public void clearHits()
    {
        targetsTouched.Clear();
    }

    //Tests if we have already hit this myth during our current attack animation
    public bool validDmg(GameObject test)
    {
        for (int i = 0; i < targetsTouched.Count; i++)
        {
            if (targetsTouched[i] == test)
            {
                return false;
            }
        }

        return true;
    }

    void Start()
    {
        playercontrol = GetComponent<PlayerController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        animator = playercontrol.gameObject.GetComponent<Animator>();
        if (!audio)
        {
            audio = GetComponent<AudioSource>();
        }
        umbrellaHitBox.enabled = false;

        movement = GetComponent<movementController>();
        charModel = movement.charcterModel.transform;

        for (int i = 0; i < 50; i++)
        {
            GameObject tmp = Instantiate(xinsButthole, new Vector3(0, 0, 0), Quaternion.identity);
            xinsButtholes.Add(tmp);
        }

    }

    void Update()
    {

        //Prevent transiton to block/attack from exiting the phone
        if (phoneLock)
        {
            phoneTimer = 0.0f;
        }
        else
        {
            phoneTimer += Time.deltaTime;
        }

        //Attack Queuing
        if (!animator.GetBool("Blocking") && (phoneTimer > phoneThreshold))
        {
            //On release
            if (Input.GetMouseButtonUp(0))
            {
                if (timerToHeavy <= timeTillHeavyAttack)
                {
                    attackQueued = true;
                }
                lastAttackHeavy = false;
                timerToHeavy = 0.0f;
            }

            //On Hold
            if (Input.GetMouseButton(0))
            {
                //If user has held the button down enough trigger heavy attack
                timerToHeavy += Time.deltaTime;
                if (timerToHeavy > timeTillHeavyAttack && !animator.GetBool("Attacking") && !lastAttackHeavy)
                {
                    attackQueued = true;
                }
            }

            latetest = false;

            if (attackQueued && !animator.GetBool("Blocking") && !animator.GetBool("Stunned") && !phoneLock)
            {
                if ((playercontrol.staminaAmount >= playercontrol.staminaToAttack && timerToHeavy <= timeTillHeavyAttack) || (playercontrol.staminaAmount >= playercontrol.staminaToHeavyAttack && timerToHeavy > timeTillHeavyAttack))
                {
                    attackQueued = false;

                    animator.SetBool("Attack", true);

                    if (timerToHeavy > timeTillHeavyAttack && !lastAttackHeavy)
                    {
                        animator.SetBool("HeavyAttack", true);
                        lastAttackHeavy = true;
                    }

                }
            }
        }

        //for blocking / aiming down sight
        if (!cooldown && !phoneLock && (phoneTimer > phoneThreshold))
        {
            //shoot
            if (Input.GetAxis("Fire2") > 0.5f)
            {
                if (playercontrol.staminaAmount > blockingStamina * Time.deltaTime)
                {
                    playercontrol.ChangeStamina(-blockingStamina * Time.deltaTime);
                    blocking();

                    firemode();
                }
                else
                {
                    aimVFX.Stop();
                    movement.strafemode = false;
                    shotUI.SetActive(false);
                    crosshair.transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    crosshair.transform.GetChild(1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    crosshair.transform.GetChild(2).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    crosshair.transform.GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    cooldown = true;
                }
            }
            else
            {
                aimVFX.Stop();
                movement.strafemode = false;
                shotUI.SetActive(false);
                crosshair.transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                crosshair.transform.GetChild(1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                crosshair.transform.GetChild(2).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                crosshair.transform.GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                //animator.ResetTrigger("Block");
                animator.SetBool("Blocking", false);
                //movement.attackMovementBlock = false;
            }
        }
        else
        {
            aimVFX.Stop();
            shotUI.SetActive(false);
            crosshair.transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            crosshair.transform.GetChild(1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            crosshair.transform.GetChild(2).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            crosshair.transform.GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            //animator.SetBool("Blocking", false);
            //movement.attackMovementBlock = false;
            cooldowntimer += Time.deltaTime;
            if (cooldowntimer > cooldowntime)
            {
                //animator.ResetTrigger("Block");
                cooldown = false;
                cooldowntimer = 0.0f;
            }
        }

        //Check for if we are not attacking then allow movement
        if (!animator.GetBool("Attacking"))
        {
            movement.attackMovementBlock = false;
        }
        else
        {
            movement.attackMovementBlock = true;
        }


    }

    //currently blocking
    void blocking()
    {
        aimVFX.Play();

        movement.strafemode = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, ball))
        {
            hit.point = new Vector3(hit.point.x, movement.charcterModel.transform.position.y, hit.point.z); //look forwards
            movement.charcterModel.transform.LookAt(hit.point);
        }

        if (!animator.GetBool("Blocking"))
        {
            animator.SetTrigger("Block");
            animator.SetBool("Blocking", true);
        }

        if (canfire == true)
        {
            if ((Mathf.Abs(this.GetComponent<movementController>().moveDirCam.z) + Mathf.Abs(this.GetComponent<movementController>().moveDirCam.x)) > 0.0f)
            {
                bulletSpread = bulletSpreadRunning;
            }
            else
            {
                bulletSpread = bulletSpreadADS;
            }



            crosshair.transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            crosshair.transform.GetChild(1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            crosshair.transform.GetChild(2).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            crosshair.transform.GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else
        {

            crosshair.transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            crosshair.transform.GetChild(1).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            crosshair.transform.GetChild(2).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            crosshair.transform.GetChild(3).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

    }

    //aiming down sight
    void firemode()
    {
        latetest = true;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ammocycle++;
            if (ammocycle > 1)
            {
                ammocycle = 0;
            }
        }

        shotUI.SetActive(true);

        if (ammocycle == 0)
        {
            shotUI.transform.GetChild(0).GetComponent<Text>().text = ammo.ToString() + "/100\nRegualr shells - Q to swap";

        }
        else if (ammocycle == 1)
        {
            shotUI.transform.GetChild(0).GetComponent<Text>().text = ammoTwo.ToString() + "/100\nExplosive shells - Q to swap";

        }


        if (Input.GetMouseButtonDown(0) && canfire == true) // shoot
        {
            if (ammocycle == 0 && ammo > 0)
            {
                animator.SetTrigger("Shoot");
                ammo--;
                SaveSystemController.updateValue("ammo", ammo);

                bang();
            }
            else if (ammocycle == 1 && ammoTwo > 0)
            {
                animator.SetTrigger("Shoot");
                ammoTwo--;
                SaveSystemController.updateValue("ammoTwo", ammoTwo);

                bang();
            }
            else
            {
                StartCoroutine(empty());
            }

        }

        
        

    }

    //shoot
    void bang()
    {
        shootVFX.Play();
        //shake
        Vector3 passTargetPos = new Vector3(0.0f, 0.1f, -0.3f);
        float passOverallSpeed = 3.0f;
        Vector3 passTargetRot = new Vector3(-3.0f, 2.0f, 0.0f);
        shakeOperation.lerpModes funcin = shakeOperation.lerpModes.OUTEXPO;
        shakeOperation.lerpModes funcout = shakeOperation.lerpModes.INSINE;
        float speedIn = 5000.0f;
        float speedOut = 1.0f;
        cam.GetComponent<cameraShake>().addOperation(passTargetPos, passTargetRot, passOverallSpeed, funcin, funcout, speedIn, speedOut);


        Transform brella = this.transform.GetChild(1).GetChild(6);
        var cameraThingTransform = cam.transform.parent.parent.transform;
        for (int j = 0; j < pellets; j++)
        {
            RaycastHit[] Hits;

            //ranodm rotations
            float yrand = Random.Range(bulletSpread, -bulletSpread);
            float xrand = Random.Range(Mathf.Sqrt(Mathf.Pow(bulletSpread, 2) - Mathf.Pow(yrand, 2)), -Mathf.Sqrt(Mathf.Pow(bulletSpread, 2) - Mathf.Pow(yrand, 2)));
            Vector3 vec3dir = new Vector3(xrand, yrand, 1);

            //adjust rotations
            Vector3 localDirection = Quaternion.Euler(cameraThingTransform.localEulerAngles.x, 0, 0) *  vec3dir;
            Quaternion correction = Quaternion.Euler(0, -90, 0);
            Vector3 worldDirection = brella.TransformDirection(correction * localDirection);


            //raycast eveything (allows wallbangs)
            Hits = Physics.RaycastAll(brella.transform.position, worldDirection, MaxRange);

            //debug
            for (int k = 0; k < Hits.Length; k++)
            {
                Debug.DrawLine(brella.transform.position, Hits[k].point, Color.white, 10.0f);
            }

            //for all hits
            for (int i = 0; i < Hits.Length; i++)
            {
                RaycastHit Hit = Hits[i];
                var go = Hit.collider.gameObject;
                if (go.CompareTag("Myth") || go.CompareTag("Boss") || go.CompareTag("Dummy"))
                {
                    //angle for bullethole
                    GameObject enemy = Hit.collider.gameObject;
                    Vector3 hitposition = Hit.point + (Hit.normal * 0.4f);

                    //damage calculation
                    float dist = Vector3.Distance(this.gameObject.transform.position, Hit.point);
                    float falloff = Mathf.Clamp(1.5f * Mathf.Cos(Mathf.Pow(dist / MaxRange, 0.3f) * (Mathf.PI / 2)), 0.0f, 1.0f);
                    float damage = falloff * (MaxDamage / (float)pellets);

                    //apply damage
                    if (Hit.collider.GetComponent<AIObject>())
                    {
                        float revealAmount = 0.0f;

                        if (Hit.collider.gameObject.tag == "Boss")
                        {
                            revealAmount = Hit.collider.GetComponent<AIObject>().revealpersentobject.GetComponent<drawTest>().blackpersent;
                        }


                        revealAmount = Hit.collider.GetComponent<AIObject>().startHealth * revealAmount;

                        float diff = (Hit.collider.GetComponent<AIObject>().health - revealAmount);

                        if (damage < diff)
                        {
                            GameObject tmp = GameObject.Instantiate(damagedText, Hit.point, Quaternion.identity);
                            tmp.transform.SetParent(Hit.collider.gameObject.transform, true);
                            tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + damage.ToString("F0");
                            Hit.collider.GetComponent<AIObject>().health -= damage;

                        }
                        else
                        {
                            GameObject tmp = GameObject.Instantiate(damagedText, Hit.point, Quaternion.identity);
                            tmp.transform.SetParent(Hit.collider.gameObject.transform, true);
                            tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + diff.ToString("F0");
                            Hit.collider.GetComponent<AIObject>().health = revealAmount;
                        }

                        break;
                    }
                    else if (Hit.collider.GetComponent<traningDummy>())
                    {
                        GameObject tmp = GameObject.Instantiate(damagedText, Hit.point, Quaternion.identity);
                        tmp.transform.SetParent(Hit.collider.gameObject.transform, true);
                        tmp.transform.GetChild(0).GetComponent<Text>().text = "-" + damage.ToString("F0");
                    }
                }

                //spawn bullet hole
                if (Hit.collider.gameObject.layer == 0) //ground and wall
                {
                    Quaternion hitRotation = Quaternion.FromToRotation(Vector3.forward, Hit.normal);
                    Vector3 hitposition = Hit.point + (Hit.normal * 0.01f);

                    //move pre spawned holes into position
                    xinsButtholes[buttholeToMove].transform.position = hitposition;
                    xinsButtholes[buttholeToMove].transform.rotation = hitRotation;
                    buttholeToMove++;
                    if (buttholeToMove > xinsButtholes.Count - 1)
                    {
                        buttholeToMove = 0;
                    }

                }
            }
        }


        movement.strafemode = false;
        cooldown = true;
    }


    public void Hitbox(bool toggle)
    {
        umbrellaHitBox.enabled = toggle;
    }


    void LateUpdate()
    {
        //make umbrella look in the corerect direction
        if (latetest == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, ball))
            {
                umbeaalBone.transform.LookAt(hit.point);
                umbeaalBone.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
                Debug.DrawLine(hit.point, cam.transform.position);
            }
        }
    }

    public IEnumerator empty()
    {
        audio.PlayOneShot(noammoClip);
        shotUI.transform.GetChild(0).GetComponent<Text>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        shotUI.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        yield return null;
    }
}
