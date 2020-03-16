using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{

    public float moveSpeed = 10.0f;
    public float jumpForce = 10.0f;
    public float gravity = 9.41f;
    public float feetradius = 0.5f;
    public float maxFallSpeedWhileGliding = 10.0f;
    public LayerMask groundLayer;
    public Transform feet;

    private CharacterController ch;
    private bool isOnGround = true;
    private Vector3 moveDir = Vector3.zero;

    private void Start()
    {
        ch = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        isOnGround = Physics.CheckSphere(feet.position, feetradius, groundLayer);
    }

    // Update is called once per frame
    void Update()
    {

        //While we are on the ground
        if (isOnGround)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")) * moveSpeed;

            if (Input.GetButton("Jump"))
            {
                moveDir.y += jumpForce;
            }
        }
        //While we are in the air
        else
        {
            //Move half speed
            moveDir = new Vector3((Input.GetAxis("Horizontal") * moveSpeed) * 0.5f, moveDir.y, (Input.GetAxis("Vertical") * moveSpeed) * 0.5f);

            //Apply Gravity
            moveDir.y -= gravity * Time.deltaTime;

            //Glide if falling and holding jump
            if (Input.GetButton("Jump") && (moveDir.y < 0))
            {
                Debug.Log("Trigger Glide");
                moveDir.y = Mathf.Clamp((moveDir.y), -maxFallSpeedWhileGliding, 0.0f);
            }

        }


        //Move
        ch.Move(moveDir * Time.deltaTime);
    }
}
