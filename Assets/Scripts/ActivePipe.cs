using System.Collections;
using UnityEngine;

public class ActivePipe : MonoBehaviour
{
    [Header("Pipe Settings")]
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private GameObject m_TeleportPoint;
    [SerializeField] private GameObject m_TeleportPoint2;
    [SerializeField] private ActivePipe m_otherPipe;
    [SerializeField] private bool m_deactivatOnTeleport;

    private GameManager gameManager;
    private bool k_isTeleporting;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !k_isTeleporting)
        {
            StartCoroutine(MoveIntoPipe(collision.gameObject));
        }
    }

    private IEnumerator MoveIntoPipe(GameObject player)
    {
        k_isTeleporting = true;
        ActivatePlayer(player, false);
        gameManager.FreezeCinema();
        yield return StartCoroutine(IntoPipe(player));
        StartCoroutine(m_otherPipe.MoveOutOfPipe(player));
        if (!m_deactivatOnTeleport) { k_isTeleporting = false; }
    }

    private IEnumerator IntoPipe(GameObject player)
    {
        Vector3 startPosition = player.transform.position;
        Vector3 pipePosition = m_TeleportPoint2.transform.position;

        float elapsedTime = 0f;
        while (player.transform.position != pipePosition)
        {
            elapsedTime += Time.fixedDeltaTime; 
            player.transform.position = Vector3.Lerp(startPosition, pipePosition, elapsedTime / m_moveSpeed);
            yield return null;
        }
    }

    private IEnumerator MoveOutOfPipe(GameObject player)
    {
        k_isTeleporting = true;
        gameManager.SetCinema(player);
        yield return StartCoroutine(OutOfPipe(player));
        ActivatePlayer(player, true);
        if (!m_deactivatOnTeleport) { k_isTeleporting = false; }
    }

    private IEnumerator OutOfPipe(GameObject player)
    {
        player.transform.position = m_TeleportPoint2.transform.position;

        Vector3 startPosition = player.transform.position;
        Vector3 pipePosition = m_TeleportPoint.transform.position;

        float elapsedTime = 0f;
        while (player.transform.position != pipePosition)
        {
            elapsedTime += Time.fixedDeltaTime;
            player.transform.position = Vector3.Lerp(startPosition, pipePosition, elapsedTime / m_moveSpeed);
            yield return null;
        }
    }

    private void ActivatePlayer(GameObject player ,bool active)
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody2D>().gravityScale = active ? 3 : 0;
        player.GetComponent<Collider2D>().enabled = active;
        player.GetComponent<CharacterController2D>().enabled = active;
        player.GetComponent<PlayerController>().enabled = active;
        player.GetComponent<Animator>().enabled = active;
    }
}
