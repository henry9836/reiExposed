using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class movementController : MonoBehaviour
{

    public float moveSpeed = 10.0f;
    public float sprintSpeedMultipler = 2.0f;
    public float jumpForce = 10.0f;
    public float gravity = 9.41f;
    public float maxFallSpeedWhileGliding = 10.0f;
    public float staminaCostSprint = 2.0f;
    public float staminaCostDash = 30.0f;
    public float staminaCostJump = 30.0f;
    public float dashDistance = 10.0f;
    public float respawnThreshold = -30.0f;
    public float turnSpeed = 0.1f;
    public LayerMask groundLayer;
    public GameObject charcterModel;
    public GameObject camParent;
    public Image sprintLines;
    public Transform feet;
    public Vector3 feetBox;

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


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawCube(feet.position, feetBox);
    }

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
       isOnGround = Physics.CheckBox(feet.position, feetBox, Quaternion.identity, groundLayer);

        if (isOnGround != previousState)
        {
            if (isOnGround)
            {
                animator.SetTrigger("jumpLand");
            }
        }
        previousState = isOnGround;

        //Fell out of map
        if (transform.position.y < respawnThreshold)
        {
            transform.position = initalPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

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
        if (moveDirCam != Vector3.zero)
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


            // =======
            // REMOVED
            // =======
            //Glide if falling and holding jump
            //if (Input.GetButton("Jump") && (moveDir.y < 0))
            //{
            //    moveDir.y = Mathf.Clamp((moveDir.y), -maxFallSpeedWhileGliding, 0.0f);
            //    animator.SetTrigger("gliding 0");
            //    animator.ResetTrigger("gliding 1");
            //}
            //else
            //{
            //    animator.SetTrigger("gliding 1");
            //    animator.ResetTrigger("gliding 0");

            //}


        }
        //While we are on the ground
        else
        {
            moveDir = camParent.transform.forward * ((Input.GetAxis("Vertical") * moveSpeed));
            moveDir += camParent.transform.right * ((Input.GetAxis("Horizontal") * moveSpeed));

            if (Input.GetButton("Jump") && pc.CheckStamina() >= staminaCostJump)
            {

                Debug.Log("Called");

                animator.SetTrigger("jumpUp");

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
                    audio.PlayOneShot(dashSounds[Random.Range(0, dashSounds.Count)]);
                    moveDir += new Vector3(moveDir.x * dashDistance, 0.0f, moveDir.z * dashDistance);
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
        ch.Move(moveDir * Time.deltaTime);


    }
}
