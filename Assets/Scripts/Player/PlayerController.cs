using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float runSpeed;                    // Player speed when running
    [SerializeField] private float walkSpeed;                   // Player speed when walking
    [SerializeField] private KeyCode sprintButton;              // Key to start sprinting


    private CharacterController2D controller;   // Controller for 2D Behavior
    private Animator anim;                      // Animator... what tf did u think it was
    private float speed;                        // Current Speed
    private float move;                         // Movement Value
    private bool jump;                          // Jumping?

    public float horizontal;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController2D>(); 
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        bool isDead = anim.GetBool("Dead");

        //Disables Movement of player if the player is dying
        if (!isDead)
        {
            //Set speed of player depending on if sprinting
            SetCurrentSpeed();

            //Get horizontal axis from unity input manager
            horizontal = Input.GetAxisRaw("Horizontal");

            //Set Moveforce to current speed * direction
            move = horizontal * speed;

            //If jumping then u jump
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }


            //Get values from CharacterController2D
            bool grounded = controller.IsTouchingGround();
            bool sliding = controller.IsSliding();
            float yVelocity = controller.GetVerticalVelocity();

            //Set animations based on character controller.
            anim.SetFloat("Speed", speed);
            anim.SetBool("IsGrounded", grounded);
            anim.SetBool("Sliding", sliding);
            anim.SetFloat("VelY", yVelocity);
        }
        else
        {
            move = 0;
        }
    }

    void FixedUpdate()
    {
        controller.Move(move * Time.fixedDeltaTime, false, jump, Input.GetKey(sprintButton));
        jump = false;
    }

    void SetCurrentSpeed()
    {
        //Set stationary speed
        speed = 0;

        //Check for horizontal movement
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
        {
            //Adjust for speed depending on if your running or walking
            speed = Input.GetKey(sprintButton) ? runSpeed : walkSpeed;
        }
    }
}
