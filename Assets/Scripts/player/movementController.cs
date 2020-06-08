using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movementController : MonoBehaviour
{
    [Header("Cam Mode")]
    public bool cameraMode = false;
    public float camSpeedmult = 0.5f;

    [Header("Movement")]
    public float moveSpeed = 10.0f;
    public float sprintSpeedMultipler = 2.0f;
    public float jumpForce = 10.0f;
    public AnimationCurve rollMovementOverTime;
    public float rollTime = 0.5f;
    public float rollDistance = 5.0f;
    public float feetCheckDistance = 0.5f;

    [Header("World")]
    public float gravity = 9.41f;
    public float respawnThreshold = -30.0f;

    [Header("Stamina")]
    public float staminaCostSprint = 2.0f;
    public float staminaCostRoll = 30.0f;
    public float staminaCostJump = 30.0f;

    [Header("Body Parts")]
    public Transform feet;
    public Transform rightFoot;
    public Transform leftFoot;
    public GameObject charcterModel;
    public GameObject camParent;
    public Transform leftFootTarget;
    public Transform rightFootTarget;

    [Header("Layer Masks")]
    public LayerMask groundLayer;
    public LayerMask rollObstcleLayer;

    [Header("Other")]
    public Image sprintLines;

    //Sounds
    public List<AudioClip> dashSounds = new List<AudioClip>();
    private AudioSource audio;

    private PlayerController pc;
    private CharacterController ch;
    private Animator animator;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 moveDirCam = Vector3.zero;
    private bool isOnGround = false;
    private float dashThresholdCeiling = 0.5f;
    private float dashTimer = 0.0f;
    private Vector3 initalPosition;
    private bool jumponce = false;
    private Quaternion targetRot;
    private bool previousState = true;
    private bool currentState = true;
    private bool rightFootGrounded = false;
    private bool leftFootGrounded = false;
    private bool centerFootGrounded = false;
    private RaycastHit hit;
    private Vector3 beforeRollPosition;
    private Vector3 targetRollPosition;
    private bool rolling = false;
    private float rollTimer = 0.0f;
    private float tmpRollDistance = 0.0f;

    private void Start()
    {
        initalPosition = transform.position;
        ch = GetComponent<CharacterController>();
        pc = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {

        //=========================
        //Gravity/Landing Section
        //=========================

        //Check for ground below each foot
        leftFootGrounded = (Physics.Raycast(leftFoot.position, Vector3.down, out hit, feetCheckDistance, groundLayer));
        if (leftFootGrounded)
        {
            Debug.DrawLine(leftFoot.position, hit.point, Color.cyan);
            leftFoot.position = hit.point;
        }
        else
        {
            Debug.DrawLine(leftFoot.position, leftFoot.position + (Vector3.down * feetCheckDistance), Color.red);
            leftFoot.position = leftFoot.position;
        }

        rightFootGrounded = (Physics.Raycast(rightFoot.position, Vector3.down, out hit, feetCheckDistance, groundLayer));

        if (rightFootGrounded)
        {
            Debug.DrawLine(rightFoot.position, hit.point, Color.cyan);
            rightFoot.position = hit.point;
        }
        else
        {
            Debug.DrawLine(rightFoot.position, rightFoot.position + (Vector3.down * feetCheckDistance), Color.red);
            rightFoot.position = rightFoot.position;
        }

        //Center foot is important as landing is controlled by character controller
        centerFootGrounded = (Physics.Raycast(feet.position, Vector3.down, out hit, feetCheckDistance, groundLayer));

        if (centerFootGrounded)
        {
            Debug.DrawLine(feet.position, hit.point, Color.cyan);
        }
        else
        {
            Debug.DrawLine(feet.position, feet.position + (Vector3.down * feetCheckDistance), Color.red);
        }

        //Set whether we are on the ground or not

        isOnGround = (rightFootGrounded || leftFootGrounded || centerFootGrounded);

        if (isOnGround != previousState)
        {
            if (isOnGround)
            {
                animator.SetTrigger("jumpLand");
            }
        }
        previousState = isOnGround;

        //Fell out of map, reset pos
        if (transform.position.y < respawnThreshold)
        {
            transform.position = initalPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Timers
        rollTimer += Time.unscaledDeltaTime;


        //We are dead
        if (GetComponent<PlayerController>().dead == true)
        {
            return;
        }

        //Match camera rotation to cam parent rotation
        //charcterModel.transform.rotation = camParent.transform.rotation

        //Get Cam Dir Input
        moveDirCam = Vector3.zero;
        moveDirCam += camParent.transform.forward * Input.GetAxis("Vertical");
        moveDirCam += camParent.transform.right * Input.GetAxis("Horizontal");
        moveDirCam = moveDirCam.normalized;


        //Rotate towards movement in relation to cam direction
        if (moveDirCam != Vector3.zero && !rolling)
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
            moveDir += (camParent.transform.forward * ((Input.GetAxis("Vertical") * moveSpeed))) * 0.5f;
            moveDir += (camParent.transform.right * ((Input.GetAxis("Horizontal") * moveSpeed))) * 0.5f;

            //Apply Gravity
            moveDir.y -= gravity * Time.deltaTime;


        }
        //While we are on the ground
        else
        {
            moveDir = camParent.transform.forward * ((Input.GetAxis("Vertical") * moveSpeed));
            moveDir += camParent.transform.right * ((Input.GetAxis("Horizontal") * moveSpeed));

            //Removed JUMP
            //if (Input.GetButton("Jump") && pc.CheckStamina() >= staminaCostJump && !rolling)
            //{

            //    Debug.Log("Called");

            //    animator.SetTrigger("jumpUp");

            //    moveDir.y += jumpForce;
            //    pc.ChangeStamina(-staminaCostJump);
            //}
        }


        //Rolling Mechanic
        if (Input.GetButtonDown("Roll") && !rolling)
        {
            //no rolling in camera mode
            if (cameraMode != true)
            {
                //Check stamina
                if (staminaCostSprint <= pc.staminaAmount)
                {
                    //Check if area is clear
                    tmpRollDistance = rollDistance;
                    RaycastHit hit;
                    if (Physics.Raycast(feet.transform.position, charcterModel.transform.forward, out hit, tmpRollDistance, rollObstcleLayer))
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
                }
            }

        }


        if (cameraMode != true)
        {
            //Lerp between start roll and end roll pos if we are rolling
            if (rolling)
            {
                //Move towards target
                transform.position = Vector3.Lerp(beforeRollPosition, targetRollPosition, rollMovementOverTime.Evaluate(rollTimer / rollTime));

                //Toggle off the roll once we have reached the end of the roll
                if (rollTimer >= rollTime)
                {
                    rolling = false;
                }
            }
            else if (Input.GetButton("Sprint") && isOnGround && !rolling) //Sprint
            {
                if ((moveDir.x != 0) && (moveDir.z != 0))
                {
                    //move a little more
                    if (pc.CheckStamina() >= staminaCostSprint * Time.deltaTime)
                    {
                        pc.ChangeStamina(-staminaCostSprint * Time.deltaTime);
                        moveDir += new Vector3(moveDir.x * sprintSpeedMultipler, 0.0f, moveDir.z * sprintSpeedMultipler);
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

            //Animation Off
            //Walking
            //Debug.Log(moveDir);


            if ((moveDir.x == 0) && (moveDir.z == 0))
            {
                animator.SetBool("walkin", false);
            }
            else
            {
                animator.SetBool("walkin", true);
            }
            //Sprint
            if (Input.GetButtonUp("Sprint") || !isOnGround)
            {
                animator.SetBool("Running", false);
            }
            //Move
            if (!rolling)
            {
                ch.Move(moveDir * Time.deltaTime);
            }
        }
        else
        {
            if (rolling)
            {
                //Move towards target
                transform.position = Vector3.Lerp(beforeRollPosition, targetRollPosition, rollMovementOverTime.Evaluate(rollTimer / rollTime));

                //Toggle off the roll once we have reached the end of the roll
                if (rollTimer >= rollTime)
                {
                    rolling = false;
                }
            }
            else
            {
                ch.Move(moveDir * Time.deltaTime * camSpeedmult);
            }
        }
    }
}
