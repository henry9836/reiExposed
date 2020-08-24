using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movementController : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed = 10.0f;
    public float sprintSpeedMultipler = 2.0f;
    public float jumpForce = 10.0f;
    public float useItemMoveSpeed = 0.3f;
    public AnimationCurve rollMovementOverTime;
    public float rollTime = 0.5f;
    public float rollDistance = 5.0f;
    public float feetCheckDistance = 0.5f;
    [HideInInspector]
    public bool canTurnDuringAttack = true;
    public bool strafemode = false;

    [Header("World")]
    public float gravity = 9.41f;
    public float respawnThreshold = -30.0f;

    [Header("Stamina")]
    public float timeToUnblock = 0.5f;
    public float staminaCostSprint = 2.0f;
    public float staminaCostRoll = 30.0f;
    public float staminaCostJump = 30.0f;

    [Header("Body Parts")]
    public GameObject charcterModel;
    public GameObject camParent;
    public Transform rollCheckTransform;

    [Header("Layer Masks")]
    public LayerMask groundLayer;
    public LayerMask rollObstcleLayer;

    [Header("Other")]
    public Image sprintLines;

    //Hidden
    [HideInInspector]
    public bool attackMovementBlock = false;

    //Sounds
    public List<AudioClip> dashSounds = new List<AudioClip>();
    private AudioSource audio;

    private PlayerController pc;
    private CharacterController ch;
    private Animator animator;
    private CapsuleCollider playerHitBox;
    [HideInInspector]
    public Vector3 moveDir = Vector3.zero;
    private Vector3 moveDirCam = Vector3.zero; 
    private Vector3 initalPosition;
    private Vector3 beforeRollPosition;
    private Vector3 targetRollPosition;
    private Quaternion targetRot;

    //Hit box
    private float startHitBoxH = 0.0f;
    private float startHitBoxY = 0.0f;
    private float rollHitBoxH = 0.87f;
    private float rollHitBoxY = 0.41f;

    private bool isOnGround = false;
    private bool rolling = false;
    private bool sprinting = false;
    private bool sprintLock = false;
    private float rollTimer = 0.0f;
    private float tmpRollDistance = 0.0f;
    private float unblockTimer = 0.0f;

    private void Start()
    {
        initalPosition = transform.position;
        ch = GetComponent<CharacterController>();
        pc = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        playerHitBox = GetComponent<CapsuleCollider>();
        unblockTimer = timeToUnblock;
        startHitBoxH = playerHitBox.height;
        startHitBoxY = playerHitBox.center.y;
    }

    private void FixedUpdate()
    {
        isOnGround = ch.isGrounded;

        //Fell out of map, reset pos
        if (transform.position.y < respawnThreshold)
        {
            transform.position = initalPosition;
        }
    }

    public void forceMovement(Vector3 dir)
    {
        ch.Move(dir * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {

        //Reset bools
        sprinting = false;

        //Timers
        rollTimer += Time.unscaledDeltaTime;


        //We are dead
        if (GetComponent<PlayerController>().dead == true)
        {
            return;
        }

        //Get Cam Dir Input
        moveDirCam = Vector3.zero;
        moveDirCam += camParent.transform.forward * Input.GetAxis("Vertical");
        moveDirCam += camParent.transform.right * Input.GetAxis("Horizontal");
        moveDirCam = moveDirCam.normalized;


        //Rotate towards movement in relation to cam direction
        //if (moveDirCam != Vector3.zero && !rolling && !strafemode && !attackMovementBlock && !animator.GetBool("KnockedDown"))
        if (moveDirCam != Vector3.zero && !rolling && !strafemode && (!attackMovementBlock || canTurnDuringAttack) && !animator.GetBool("KnockedDown"))
        {

            //Get cam rotation
            Vector3 camRot = camParent.transform.rotation.eulerAngles;

            //Rotate character model to match cam
            charcterModel.transform.rotation = camParent.transform.rotation; ;

            //Offset rotation to movement direction
            //Offset target
            Vector3 offset = new Vector3(camParent.transform.position.x + (moveDirCam.x * 10.0f), charcterModel.transform.position.y, camParent.transform.position.z + (moveDirCam.z * 10.0f));

            //Offset rotation
            targetRot = Quaternion.LookRotation((offset - charcterModel.transform.position).normalized);
            Vector3 targetDir = (offset - charcterModel.transform.position).normalized;
            //Rotation
            charcterModel.transform.LookAt(offset, Vector3.up);

        }

        //While we are in the air
        if (!isOnGround)
        {
            //Move half speed
            moveDir = new Vector3(0.0f, moveDir.y, 0.0f);

            if (!animator.GetBool("UsingItem") && !animator.GetBool("KnockedDown"))
            {
                moveDir += camParent.transform.forward * ((Input.GetAxis("Vertical") * moveSpeed));
                moveDir += camParent.transform.right * ((Input.GetAxis("Horizontal") * moveSpeed));
            }
            else
            {
                moveDir += camParent.transform.forward * ((Input.GetAxis("Vertical") * useItemMoveSpeed));
                moveDir += camParent.transform.right * ((Input.GetAxis("Horizontal") * useItemMoveSpeed));
            }
            //Apply Gravity
            moveDir.y -= gravity * Time.deltaTime;


        }
        //While we are on the ground
        else
        {
            if (!animator.GetBool("UsingItem") && !animator.GetBool("KnockedDown"))
            {
                moveDir = camParent.transform.forward * ((Input.GetAxis("Vertical") * moveSpeed));
                moveDir += camParent.transform.right * ((Input.GetAxis("Horizontal") * moveSpeed));
            }
            else
            {
                moveDir = camParent.transform.forward * ((Input.GetAxis("Vertical") * useItemMoveSpeed));
                moveDir += camParent.transform.right * ((Input.GetAxis("Horizontal") * useItemMoveSpeed));
            }
        }

        //Rolling Mechanic
        if (Input.GetButtonDown("Roll") && !rolling && !animator.GetBool("UsingItem") && !animator.GetBool("KnockedDown"))
        {
            //Check stamina
            if (staminaCostRoll <= pc.staminaAmount)
            {
                //Check if area is clear
                tmpRollDistance = rollDistance;
                RaycastHit hit;
                if (Physics.Raycast(rollCheckTransform.position, charcterModel.transform.forward, out hit, tmpRollDistance, rollObstcleLayer))
                {
                    //If we hit something then only roll to just before the object we hit
                    tmpRollDistance = hit.distance - 1.0f;
                }
                //Roll in the forward direction of model
                targetRollPosition = transform.position + (charcterModel.transform.forward * tmpRollDistance);
                beforeRollPosition = transform.position;

                //Reset timer
                rollTimer = 0.0f;

                //Stamina
                pc.ChangeStamina(-staminaCostRoll);

                //Lock other movement until roll is complete
                rolling = true;

                //Change hitbox
                playerHitBox.height = rollHitBoxH;
                playerHitBox.center = new Vector3(playerHitBox.center.x, rollHitBoxY, playerHitBox.center.z);


                //Animation
                animator.SetBool("Rolling", true);
                animator.SetTrigger("Roll");
            }
        }

        //Lerp between start roll and end roll pos if we are rolling
        if (rolling)
        {
            //Move towards target
            transform.position = Vector3.Lerp(beforeRollPosition, targetRollPosition, rollMovementOverTime.Evaluate(rollTimer / rollTime));

            //Toggle off the roll once we have reached the end of the roll
            if (rollTimer >= rollTime)
            {
                rolling = false;

                //Change hitbox
                playerHitBox.height = startHitBoxH;
                playerHitBox.center = new Vector3(playerHitBox.center.x, startHitBoxY, playerHitBox.center.z);

                //Animation
                animator.SetBool("Rolling", false);
                animator.ResetTrigger("Roll");
            }
        }

        //Sprint
        else if (Input.GetButton("Sprint") && isOnGround && !rolling && !sprintLock && !animator.GetBool("UsingItem") && !animator.GetBool("KnockedDown"))
        {
            if ((moveDir.x != 0) && (moveDir.z != 0))
            {
                //move a little more
                if (pc.CheckStamina() >= staminaCostSprint * Time.deltaTime)
                {
                    sprinting = true;

                    pc.ChangeStamina(-staminaCostSprint * Time.deltaTime);
                    moveDir += new Vector3(moveDir.x * sprintSpeedMultipler, 0.0f, moveDir.z * sprintSpeedMultipler);

                    //If we have run out of stmina lock sprinting
                    if(pc.CheckStamina() < (staminaCostSprint * Time.deltaTime))
                    {
                        sprintLock = true;
                    }

                    //Animation
                    animator.SetBool("Running", true);

                    float alpha = sprintLines.material.GetFloat("Vector1_BD31B2DE");
                    alpha = Mathf.Clamp((alpha + (Time.unscaledDeltaTime * 2.5f)), 0.0f, 1.0f);
                    sprintLines.material.SetFloat("Vector1_BD31B2DE", alpha);
                }
            }
        }

        if (!Input.GetButton("Sprint") || !isOnGround || !(pc.CheckStamina() >= staminaCostSprint))
        {
            float alpha = sprintLines.material.GetFloat("Vector1_BD31B2DE");
            alpha = Mathf.Clamp((alpha - (Time.unscaledDeltaTime * 2.5f)), 0.0f, 1.0f);
            sprintLines.material.SetFloat("Vector1_BD31B2DE", alpha);
        }

        //Animation
        //Walking

        if ((moveDir.x == 0) && (moveDir.z == 0))
        {
            animator.SetBool("Running", false);
        }
        else
        {
            animator.SetBool("Running", true);
        }
        //Sprint
        if (Input.GetButton("Sprint") && pc.CheckStamina() >= staminaCostSprint)
        {
            animator.SetBool("Sprinting", true);
        }
        else
        {
            animator.SetBool("Sprinting", false);
        }

        //Debug.Log("BLOCK: " + attackMovementBlock.ToString());

        //Move
        if (!rolling && !attackMovementBlock)
        {
            ch.Move(moveDir * Time.deltaTime);
        }

        //Stamina Block Timer
        if ((rolling || sprinting || attackMovementBlock || sprintLock || animator.GetBool("Attack")))
        {
            unblockTimer = 0.0f;
        }
        else
        {
            unblockTimer += Time.deltaTime;
        }

        //Stamina Block
        pc.staminaBlock = (unblockTimer < timeToUnblock);

        //Sprinting lock
        if (sprintLock && !Input.GetButton("Sprint"))
        {
            sprintLock = false;
        }
    }
}
