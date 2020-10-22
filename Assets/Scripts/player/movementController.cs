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

    [Header("Body Parts")]
    public GameObject charcterModel;
    public GameObject camParent;
    public Transform rollCheckTransform;

    [Header("Layer Masks")]
    public LayerMask groundLayer;
    public LayerMask rollObstcleLayer;

    [Header("Other")]
    public Image sprintLines;

    //cam shake
    private GameObject cam;
    private float shakeTimer = 0.0f;
    private int shakenumber = 0;

    //Hidden
    [HideInInspector]
    public bool attackMovementBlock = false;

    //Sounds
    public List<AudioClip> dashSounds = new List<AudioClip>();
    public List<AudioClip> Footsteps = new List<AudioClip>();
    private AudioSource audio;

    private PlayerController pc;
    private CharacterController ch;
    private Animator animator;
    private CapsuleCollider playerHitBox;
    [HideInInspector]
    public Vector3 moveDir = Vector3.zero;
    [HideInInspector]
    public Vector3 moveDirCam = Vector3.zero; 
    private Vector3 initalPosition;
    private Vector3 beforeRollPosition;
    private Vector3 targetRollPosition;
    private Quaternion targetRot;

    //Hit box
    private float startHitBoxH = 0.0f;
    private float startHitBoxY = 0.0f;
    private float rollHitBoxH = 0.87f;
    private float rollHitBoxY = 0.41f;

    public bool isOnGround = false;
    private bool rolling = false;
    private bool sprinting = false;
    private bool sprintLock = false;
    private float rollTimer = 0.0f;
    private float tmpRollDistance = 0.0f;
    private float unblockTimer = 0.0f;
    private float rollSpeed = 0.0f;

    private bool disallowPlayerFromStamina = false;
    private bool staminaBlockState = false;

    private Vector3 lastInputDir = Vector3.zero;
    private Vector3 currentInputDir = Vector3.zero;
    private Vector3 lastOffset = Vector3.zero;
    private Vector3 offset = Vector3.zero;
    private float rotTimer = 0.0f;
    private float rotTime = 0.1f;


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
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void forceMovement(Vector3 dir)
    {
        ch.Move(dir * Time.deltaTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //checks if we are on the ground
        isOnGround = ch.isGrounded;

        //Fell out of map, reset pos
        if (transform.position.y < respawnThreshold)
        {
            transform.position = initalPosition;
        }

        //Reset movedir
        moveDir = new Vector3(moveDir.x * 0.0f, moveDir.y * 1.0f, moveDir.z * 0.0f);

        //Reset y velocity if on ground
        if (isOnGround)
        {
            moveDir.y = 0.0f;
        }

        //Apply Gravity
        moveDir.y -= gravity * Time.deltaTime;

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
        moveDirCam += camParent.transform.forward * Input.GetAxisRaw("Vertical");
        moveDirCam += camParent.transform.right * Input.GetAxisRaw("Horizontal");
        moveDirCam = moveDirCam.normalized;

        //Rotate towards movement in relation to cam direction
        if (moveDirCam != Vector3.zero && !rolling && !strafemode && (!attackMovementBlock || canTurnDuringAttack) && !animator.GetBool("Stunned") && !animator.GetBool("Blocking"))
        {
            //If this is a new direction
            //if (moveDirCam != currentInputDir && (rotTimer > rotTime))
            if (moveDirCam != currentInputDir)
            {
                //Update last input direction
                lastInputDir = currentInputDir;
                rotTimer = 0.0f;

                //Update new input direction
                currentInputDir = moveDirCam;
            }
        }

        //Smoothly rotate towards the new direction
        if (rotTimer <= rotTime)
        {
            //Offset rotation to movement direction
            //Get Input Offset target
            offset = new Vector3(camParent.transform.position.x + (currentInputDir.x * 10.0f), charcterModel.transform.position.y, camParent.transform.position.z + (currentInputDir.z * 10.0f));

            //Get Last Input Offset target
            lastOffset = new Vector3(camParent.transform.position.x + (lastInputDir.x * 10.0f), charcterModel.transform.position.y, camParent.transform.position.z + (lastInputDir.z * 10.0f));

            offset = Vector3.Lerp(lastOffset, offset, (rotTimer / rotTime));

            //Offset rotation
            targetRot = Quaternion.LookRotation((offset - charcterModel.transform.position).normalized);

            //Rotation
            charcterModel.transform.LookAt(offset, Vector3.up);
        }

        //Increment the rotation timer
        rotTimer += Time.deltaTime;

        if (!animator.GetBool("UsingItem") && !animator.GetBool("Stunned"))
        {
            moveDir += camParent.transform.forward * ((Input.GetAxis("Vertical") * moveSpeed));
            moveDir += camParent.transform.right * ((Input.GetAxis("Horizontal") * moveSpeed));
        }
        //If using an item
        else if (animator.GetBool("UsingItem"))
        {
            moveDir += camParent.transform.forward * ((Input.GetAxis("Vertical") * useItemMoveSpeed));
            moveDir += camParent.transform.right * ((Input.GetAxis("Horizontal") * useItemMoveSpeed));

            //Audio
            if (!audio.isPlaying && animator.GetCurrentAnimatorStateInfo(0).IsName("ConsumeWalk"))
            {
                audio.PlayOneShot(Footsteps[4]);
            }
        }

        //Rolling Mechanic
        if (Input.GetButtonDown("Roll") && !rolling && !animator.GetBool("UsingItem") && !animator.GetBool("Stunned") && !disallowPlayerFromStamina)
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

                //Roll depending on input
                if (moveDirCam == Vector3.zero)
                {
                    //Roll in the forward direction of model
                    targetRollPosition = transform.position + (charcterModel.transform.forward * tmpRollDistance);
                    beforeRollPosition = transform.position;
                }
                else
                {
                    //Roll in forward direction of input
                    beforeRollPosition = transform.position;
                    targetRollPosition = transform.position + (moveDirCam * tmpRollDistance);
                }

                //Reset timer
                rollTimer = 0.0f;

                //Stamina
                pc.ChangeStamina(-staminaCostRoll);

                //Lock other movement until roll is complete
                rolling = true;

                //Change hitbox
                playerHitBox.height = rollHitBoxH;
                playerHitBox.center = new Vector3(playerHitBox.center.x, rollHitBoxY, playerHitBox.center.z);

                //Get speed
                rollSpeed = Vector3.Distance(targetRollPosition, beforeRollPosition);

                //Animation
                animator.SetBool("Rolling", true);
                animator.SetTrigger("Roll");
            }
        }

        //Lerp between start roll and end roll pos if we are rolling
        if (rolling)
        {

            //Allow rotation change for slow input
            if ((rollTimer / rollTime) < 0.4f) {
                //If there is input
                if (moveDirCam != Vector3.zero)
                {
                    //Debug.Log("Roll ADJUST");
                    //Get cam rotation
                    Vector3 camRot = camParent.transform.rotation.eulerAngles;

                    //Rotate character model to match cam
                    charcterModel.transform.rotation = camParent.transform.rotation;

                    //Offset rotation to movement direction
                    //Offset target
                    Vector3 offset = new Vector3(camParent.transform.position.x + (moveDirCam.x * 10.0f), charcterModel.transform.position.y, camParent.transform.position.z + (moveDirCam.z * 10.0f));


                    //Offset rotation
                    targetRot = Quaternion.LookRotation((offset - charcterModel.transform.position).normalized);
                    Vector3 targetDir = (offset - charcterModel.transform.position).normalized;

                    //Rotation
                    charcterModel.transform.LookAt(offset, Vector3.up);

                    //Get new target position to go to
                    targetRollPosition = beforeRollPosition + (moveDirCam * tmpRollDistance);
                }
            }

            //Move towards roll target
            Vector3 dir = (targetRollPosition - beforeRollPosition).normalized;
            dir += new Vector3(0.0f, 1.0f * moveDir.y, 0.0f);
            ch.Move(dir * (rollSpeed * 3.0f) * rollMovementOverTime.Evaluate(rollTimer / rollTime) * Time.deltaTime);

            //Move towards target
            //transform.position = Vector3.Lerp(beforeRollPosition + new Vector3(0.0f, 1.0f * moveDir.y, 0.0f), targetRollPosition + new Vector3(0.0f, 1.0f * moveDir.y, 0.0f), rollMovementOverTime.Evaluate(rollTimer / rollTime));
            //transform.position += new Vector3(0.0f, 1.0f * moveDir.y, 0.0f);

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
        else if (Input.GetButton("Sprint") && isOnGround && !rolling && !sprintLock && !animator.GetBool("UsingItem") && !animator.GetBool("Stunned") && ((moveDir.x != 0) && (moveDir.z != 0)) && (pc.CheckStamina() >= staminaCostSprint * Time.deltaTime) && !disallowPlayerFromStamina)
        {
             //move a little more

             sprinting = true;


             pc.ChangeStamina(-staminaCostSprint * Time.deltaTime);
             moveDir += new Vector3(moveDir.x * sprintSpeedMultipler, 0.0f, moveDir.z * sprintSpeedMultipler);

             //If we have run out of stmina lock sprinting
             if(pc.CheckStamina() < (staminaCostSprint * Time.deltaTime))
             {
                 sprintLock = true;
             }

             //Animation
             animator.SetBool("Sprinting", true);
             animator.SetBool("Running", false);

            //Audio
            //if (!audio.isPlaying)
            //{
            //    audio.PlayOneShot(Footsteps[Random.Range(2, 5)]);
            //}


            //camshake
            shakeTimer += Time.deltaTime;
             if (shakeTimer > 0.285)
             {
                 shakeTimer = 0.0f;

                 if (shakenumber == 0)
                 {
                     shakenumber = 1;

                     Vector3 passTargetRot = new Vector3(0.0f, 0.0f, 0.0f);
                     float passOverallSpeed = 3.50877192982f;
                     Vector3 passTargetPos = new Vector3(0.075f, 0.035f, 0.0f);
                     cam.GetComponent<cameraShake>().addOperation(passTargetPos, passTargetRot, passOverallSpeed, shakeOperation.lerpModes.OUTSINE, shakeOperation.lerpModes.INSINE, 1.0f, 1.0f);
                 }
                 else if (shakenumber == 1)
                 {
                     shakenumber = 0;

                     Vector3 passTargetRot = new Vector3(0.0f, 0.0f, 0.0f);
                     float passOverallSpeed = 3.50877192982f;
                     Vector3 passTargetPos = new Vector3(-0.075f, 0.035f, 0.0f);
                     cam.GetComponent<cameraShake>().addOperation(passTargetPos, passTargetRot, passOverallSpeed, shakeOperation.lerpModes.OUTSINE, shakeOperation.lerpModes.INSINE, 1.0f, 1.0f);
                 }
                 else
                 {
                     shakenumber = 0;
                 }


             }


             float alpha = sprintLines.material.GetFloat("Vector1_BD31B2DE");
             alpha = Mathf.Clamp((alpha + (Time.deltaTime * 10.0f)), 0.0f, 0.15f);
             sprintLines.material.SetFloat("Vector1_BD31B2DE", alpha);
             
              
        }
        else
        {
            animator.SetBool("Sprinting", false);

            float alpha = sprintLines.material.GetFloat("Vector1_BD31B2DE");
            alpha = Mathf.Clamp((alpha - (Time.deltaTime * 10.0f)), 0.0f, 0.15f);
            sprintLines.material.SetFloat("Vector1_BD31B2DE", alpha);


            //Walking
            if ((moveDir.x == 0) && (moveDir.z == 0))
            {
                animator.SetBool("Running", false);
            }
            else
            {
                //cam shake
                shakeTimer += Time.deltaTime;
                if (shakeTimer > 0.36)
                {
                    shakeTimer = 0.0f;
                    shakeTimer = 0.0f;

                    if (shakenumber == 0)
                    {
                        shakenumber = 1;

                        Vector3 passTargetRot = new Vector3(0.0f, 0.0f, 0.0f);
                        float passOverallSpeed = 2.77777f;
                        Vector3 passTargetPos = new Vector3(0.02f, -0.01f, 0.0f);
                        cam.GetComponent<cameraShake>().addOperation(passTargetPos, passTargetRot, passOverallSpeed, shakeOperation.lerpModes.OUTSINE, shakeOperation.lerpModes.INSINE, 1.0f, 1.0f);
                    }
                    else if (shakenumber == 1)
                    {
                        shakenumber = 0;

                        Vector3 passTargetRot = new Vector3(0.0f, 0.0f, 0.0f);
                        float passOverallSpeed = 2.77777f;
                        Vector3 passTargetPos = new Vector3(-0.02f, -0.01f, 0.0f);
                        cam.GetComponent<cameraShake>().addOperation(passTargetPos, passTargetRot, passOverallSpeed, shakeOperation.lerpModes.OUTSINE, shakeOperation.lerpModes.INSINE, 1.0f, 1.0f);
                    }
                    else
                    {
                        shakenumber = 0;
                    }
                }

                animator.SetBool("Running", true);
                //if (!audio.isPlaying)
                //{
                //    audio.PlayOneShot(Footsteps[Random.Range(0, 2)]);
                //}
            }
        }

        //Strafing
        if (animator.GetBool("Blocking"))
        {
            Vector3 theDir = Vector3.zero;
            theDir += Vector3.up * Input.GetAxis("Vertical");
            theDir += Vector3.right * Input.GetAxis("Horizontal");
            theDir = theDir.normalized;

            //Debug.Log(theDir);

            animator.SetBool("SRight", false);
            animator.SetBool("SLeft", false);
            animator.SetBool("SForward", false);
            animator.SetBool("SBackward", false);

            //Moving Animation
            if (theDir.y > 0)
            {
                animator.SetBool("SForward", true);
                animator.SetBool("Strafing", true);
            }
            else if (theDir.y < 0)
            {
                animator.SetBool("SBackward", true);
                animator.SetBool("Strafing", true);
            }
            else if(theDir.x > 0)
            {
                animator.SetBool("SRight", true);
                animator.SetBool("Strafing", true);
            }
            else if (theDir.x < 0)
            {
                animator.SetBool("SLeft", true);
                animator.SetBool("Strafing", true);
            }
            else
            {
                animator.SetBool("Strafing", false);
            }

            //Move
            if (!rolling && !attackMovementBlock)
            {
                ch.Move(moveDir * Time.deltaTime * 0.5f);
            }

            //Audio
            if (animator.GetBool("Strafing") && !audio.isPlaying)
            {
                audio.PlayOneShot(Footsteps[4]);
            }

        }
        else
        {
            //Move
            if (!rolling && !attackMovementBlock)
            {
                ch.Move(moveDir * Time.deltaTime);
            }
        }


        //Get states that would block stamina regen
        staminaBlockState = (rolling || sprinting || attackMovementBlock || sprintLock || animator.GetBool("Attack") || animator.GetBool("Blocking"));

        //Reset stamina blocker if the player is not using it
        if (!Input.GetButton("Sprint") && !Input.GetButton("Roll") && !staminaBlockState)
        {
            disallowPlayerFromStamina = false;
        }
        //Check if we have used too much stamina if so stop the user from using stamina so they can regen
        else if ((pc.CheckStamina() < staminaCostSprint * Time.deltaTime) || (pc.CheckStamina() < staminaCostRoll * Time.deltaTime))
        {
            disallowPlayerFromStamina = true;
        }

        //Stamina Block Timer used to regen stamina
        //Reset timer and wait for user to stop using their button unless they are blocked as they cannot use their buttons
        if (staminaBlockState && !disallowPlayerFromStamina)
        {
            unblockTimer = 0.0f;
        }
        else
        {
            unblockTimer += Time.deltaTime;
        }

        //Stamina Block
        pc.staminaBlock = (unblockTimer < timeToUnblock);

        //Sprinting lock reset
        if (sprintLock && !Input.GetButton("Sprint"))
        {
            sprintLock = false;
        }
    }
}
