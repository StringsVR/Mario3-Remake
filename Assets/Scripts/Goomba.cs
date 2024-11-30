using System.Collections;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;              // Speed of movement
    [SerializeField] private LayerMask groundLayer;             // Layer mask for the ground
    [SerializeField] private bool movingRight = false;          // Direction toggle
    [SerializeField] private float freezeDistance = 20f;        // How far away player must be from goomba to freeze
    [SerializeField] private float bounceForce = 5f;            // How high goomba should bounce

    private Rigidbody2D rb;
    private Vector2 pos;
    private Animator anim;
    private GameManager gameManager;
    private bool dying = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!dying)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                //Check if player is far away 
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < freezeDistance)
                {
                    // Move the Goomba
                    rb.velocity = new Vector2(movingRight ? moveSpeed : -moveSpeed, rb.velocity.y);
                }
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void Flip()
    {
        // Flip the direction
        movingRight = !movingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the player is above the Goomba
            if (collision.contacts[0].normal.y < -0.1f)
            {
                pos = transform.position;
                if (!dying)
                {
                    // Player stomps the Goomba
                    Die();

                    Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        // Apply an upward force to the player
                        playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
                    }
                }
            }
            else
            {
                if (!dying)
                {
                    Debug.Log("Player hit by Goomba!");
                    PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
                    player.RemoveHealth(1);
                }
            }
        }
        else if (collision.gameObject.layer != 3)
        {
            Flip();
        }
    }

    private void Die()
    {
        // Destroy the Goomba
        dying = true;
        anim.SetBool("Squished", true);

        float waitTime = 0.6f;
        float timer = 0f;
        while (timer < waitTime)
        {
            transform.position = pos;
            timer += Time.deltaTime;
        }
    }

    public void DestroyGoomba()
    {
        Destroy(gameObject);
        gameManager.AddScoreAmount(100);
    }
}
