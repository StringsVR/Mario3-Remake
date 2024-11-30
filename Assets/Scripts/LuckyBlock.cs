using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyBlock : MonoBehaviour
{
    [SerializeField] private float m_animationBumpHeight = 0.2f;
    [SerializeField] private float m_animationSpeed = 0.1f;
    [SerializeField] private Sprite m_luckyBlockSprite;
    [SerializeField] private Sprite m_luckyBlockSprite2;
    [SerializeField] private GameObject m_luckyDrop;

    private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hit)
        {
            if (collision.contacts[0].normal.y > 0.5)
            {
                hit = true;
                AnimateLuckyBlock();
            }
        }
    }

    private void AnimateLuckyBlock()
    {
        // Change the sprite to indicate the block has been hit
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        render.sprite = m_luckyBlockSprite;

        // Start the bump animation
        StartCoroutine(BumpAnimation(render));

        //
    }

    private IEnumerator BumpAnimation(SpriteRenderer render)
    {
        Vector3 originalPosition = transform.position;
        Vector3 bumpedPosition = originalPosition + new Vector3(0, m_animationBumpHeight, 0);

        // Move up
        float elapsedTime = 0f;
        while (elapsedTime < m_animationSpeed)
        {
            transform.position = Vector3.Lerp(originalPosition, bumpedPosition, elapsedTime / m_animationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = bumpedPosition;
        render.sprite = m_luckyBlockSprite2;

        // Move back down
        elapsedTime = 0f;
        while (elapsedTime < m_animationSpeed)
        {
            transform.position = Vector3.Lerp(bumpedPosition, originalPosition, elapsedTime / m_animationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;

        // Spawn the lucky drop
        Instantiate(m_luckyDrop, originalPosition + new Vector3(0, 1, 0), Quaternion.identity);
    }
}
