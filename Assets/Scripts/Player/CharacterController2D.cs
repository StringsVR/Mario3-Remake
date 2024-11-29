using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] private float m_sprintJumpForce = 600f;					// Amount of force added when the player jumps while sprinting
	[SerializeField] private float m_CrouchSpeed = .36f;						// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private float m_slidingActiveForce;						// What speed should the player start sliding?
	[SerializeField] private float m_slideFacor;								// How much the player will slide
    [SerializeField] private float m_slideThreshold;							// How much velocity until the slide stops
    [SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
    [SerializeField] private float maxJumpTime = 0.5f;							// Maximum time the jump can be held
    [SerializeField] private float jumpHoldForce = 200f;						// Additional force applied while holding jump


    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f;  // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;	// Rigidbody obv
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private bool m_isSliding;
	private float wasMove = 0;
    private float jumpTimeCounter;		// Timer to track jump duration
    private bool isJumping;             // Whether the player is currently jumping

    [System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
			}
		}
	}


	public void Move(float move, bool crouch, bool jump, bool sprinting)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
				}
			}

            // Move the character by finding the target velocity
            Vector3 targetVelocity;
            if (Mathf.Abs(move) > 0.01f && !m_isSliding)
            {
                // When there's input, set target velocity directly
                targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
                m_isSliding = false;
				wasMove = Mathf.Abs(move);
            }
            else
            {
				if (m_Grounded)
				{
                    // When no input, apply sliding effect by gradually reducing velocity
                    if (!(Mathf.Abs(m_Rigidbody2D.velocity.x) < m_slideThreshold) && wasMove > m_slidingActiveForce)
                    {
                        targetVelocity = new Vector2(m_Rigidbody2D.velocity.x * m_slideFacor, m_Rigidbody2D.velocity.y); // 0.95 controls slide factor
                        m_isSliding = true;
                    }
                    else
                    {
                        targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
                        m_isSliding = false;
                    }
                }
				else
				{
                    targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
                    m_isSliding = false;
				}
            }


            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			if (!m_isSliding)
			{
                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }

		}


        // If the player should jump...
        if (m_Grounded && jump && !m_isSliding)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
            m_Rigidbody2D.AddForce(new Vector2(0f, sprinting ? m_sprintJumpForce : m_JumpForce));
        }
    }

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public bool IsTouchingGround()
	{
		return m_Grounded;
	}

	public bool IsSliding()
	{
		return m_isSliding;
	}

	public float GetVerticalVelocity()
	{
		return m_Rigidbody2D.velocity.y;
	}
}
