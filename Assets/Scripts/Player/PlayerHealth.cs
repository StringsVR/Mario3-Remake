using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float descentSpeed;
    [SerializeField] private GameManager gameManager;
    private CapsuleCollider2D m_circleCollider;
    private Animator anim;
    private Rigidbody2D rb;

    private protected int _health = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        m_circleCollider = GetComponent<CapsuleCollider2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHealth(int amount)
    {
        this._health += amount;
    }

    public void RemoveHealth(int amount)
    {
        this._health -= amount;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Freeze Cinemachine
        gameManager.FreezeCinema();

        //Play Death Anim
        anim.SetBool("Dead", true);

        //Make him slowly descent
        m_circleCollider.isTrigger = true;
        rb.gravityScale = 0.8f;
        rb.velocity = new Vector2(0, 0);

        StartCoroutine(WaitUntilBelowY(-80));
    }

    private IEnumerator WaitUntilBelowY(float targetY)
    {
        // Wait until the player's Y position is less than or equal to the target value
        while (transform.position.y > targetY)
        {
            yield return null; // Wait until the next frame
        }

        //Reset level
        gameManager.PlayerDied();
        // Deactivate the player
        gameObject.SetActive(false);
    }

    private bool IsPlayerVisible()
    {
        return transform.position.y > -80f;
    }
}
