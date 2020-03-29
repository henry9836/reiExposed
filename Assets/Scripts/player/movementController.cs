using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{

    public float moveSpeed = 10.0f;
    public float sprintSpeedMultipler = 2.0f;
    public float jumpForce = 10.0f;
    public float gravity = 9.41f;
    public float feetradius = 0.5f;
    public float maxFallSpeedWhileGliding = 10.0f;
    public float staminaCostSprint = 2.0f;
    public float staminaCostDash = 30.0f;
    public float staminaCostJump = 30.0f;
    public float dashDistance = 10.0f;
    public LayerMask groundLayer;
    public Transform feet;
    public GameObject charcterModel;
    public GameObject camParent;

    private PlayerController pc;
    private CharacterController ch;
    private Animator animator;
    private Vector3 moveDir = Vector3.zero;
    private bool isOnGround = true;
    private float dashThresholdCeiling = 0.5f;
    private float dashTimer = 0.0f;

    private void Start()
    {
        ch = GetComponent<CharacterController>();
        pc = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        isOnGround = Physics.CheckSphere(feet.position, feetradius, groundLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PlayerController>().dead == true)
        {
            return;
        }

        charcterModel.transform.rotation = camParent.transform.rotation;

        //While we are in the air
        if (!isOnGround)
        {
            //Move half speed
            moveDir = new Vector3(0.0f, moveDir.y, 0.0f);
            moveDir += (charcterModel.transform.forward * ((Input.GetAxis("Vertical") * moveSpeed))) * 0.5f;
            moveDir += (charcterModel.transform.right * ((Input.GetAxis("Horizontal") * moveSpeed))) * 0.5f;

            //Apply Gravity
            moveDir.y -= gravity * Time.deltaTime;

            //Glide if falling and holding jump
            if (Input.GetButton("Jump") && (moveDir.y < 0))
            {
                moveDir.y = Mathf.Clamp((moveDir.y), -maxFallSpeedWhileGliding, 0.0f);
            }


        }
        //While we are on the ground
        else
        {
            moveDir = charcterModel.transform.forward * ((Input.GetAxis("Vertical") * moveSpeed));
            moveDir += charcterModel.transform.right * ((Input.GetAxis("Horizontal") * moveSpeed));

            if (Input.GetButton("Jump") && pc.CheckStamina() >= staminaCostJump)
            {
                moveDir.y += jumpForce;
                pc.ChangeStamina(-staminaCostJump);
            }
        }

        //Dash
        if (Input.GetButtonDown("Dash"))
        {
            //We are moving in a direction
            if ((moveDir.x != 0) && (moveDir.z != 0))
            {
                //move more
                if (pc.CheckStamina() >= staminaCostDash)
                {
                    moveDir += new Vector3(moveDir.x * dashDistance, moveDir.y, moveDir.z * dashDistance);
                    pc.ChangeStamina(-staminaCostDash);
                }
            }
        }

        //Sprint
        else if (Input.GetButton("Sprint") && isOnGround)
        {
            if ((moveDir.x != 0) && (moveDir.z != 0))
            {
                //move a little more
                if (pc.CheckStamina() >= staminaCostSprint)
                {
                    pc.ChangeStamina(-staminaCostSprint);
                    moveDir += new Vector3(moveDir.x * sprintSpeedMultipler, moveDir.y, moveDir.z * sprintSpeedMultipler);
                    animator.SetBool("Running", true);
                }
            }
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
        ch.Move(moveDir * Time.deltaTime);
    }
}
