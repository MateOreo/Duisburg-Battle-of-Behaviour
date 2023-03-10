using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed; 

    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        air
    }

    private void stateHandeler() 
    {

        if (PlayerCam.inDialouge == false)
        {
            if (grounded && Input.GetKey(sprintKey))
            {
                state = MovementState.sprinting;
                moveSpeed = sprintSpeed;
            }

            else if (grounded)
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
            }

            else
            {
                state = MovementState.air;
            }
        }
        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {

        if (PlayerCam.inDialouge == false)
        {
            grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

            MyInput();
            SpeedControl();
            stateHandeler();

            if (grounded)
            {
                rb.drag = groundDrag;
            }

            else
            {
                rb.drag = 0f;
            }
        }
        
        
    }

    private void FixedUpdate()
    {
        if (PlayerCam.inDialouge == false)
        {
            MovePlayer();
        }
            
    }

    private void MyInput()
    {
        if (PlayerCam.inDialouge == false)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(jumpKey))
            {
                if (readyToJump)
                {


                    if (grounded)
                    {
                        readyToJump = false;
                        Jump();

                        Invoke(nameof(ResetJump), jumpCooldown);
                    }

                }


            }
        }
        
    }

    private void MovePlayer()
    {
        if (PlayerCam.inDialouge == false)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;


            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else if (!grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }

        
        
    }

    private void SpeedControl()
    {
        if (PlayerCam.inDialouge == false)
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
            
    }

    private void Jump()
    {
        if (PlayerCam.inDialouge == false)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        }

    }

    private void ResetJump()
    {
        if (PlayerCam.inDialouge == false)
        {
            readyToJump = true;

        }
            
    }
}
