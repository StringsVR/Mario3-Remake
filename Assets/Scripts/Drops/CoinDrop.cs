using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    [SerializeField] private float m_staySpeed = 0.4f;
    [SerializeField] private float m_animationSpeed = 0.1f;
    [SerializeField] private float m_dissapearHeight = 0.5f;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(FloatNDissapear());
    }

    private IEnumerator FloatNDissapear()
    {
        Vector3 originalPosition = transform.position;
        Vector3 bumpedPosition = originalPosition + new Vector3(0, m_dissapearHeight, 0);

        // Move up
        float elapsedTime = 0f;
        while (elapsedTime < m_animationSpeed)
        {
            transform.position = Vector3.Lerp(originalPosition, bumpedPosition, elapsedTime / m_animationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = bumpedPosition;
        gameManager.AddCoinAmount(1);

        elapsedTime = 0f;
        while (elapsedTime < m_staySpeed)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
