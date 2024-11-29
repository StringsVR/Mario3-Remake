using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If comes in contact with something and its the player
        if (other.CompareTag("Player"))
        {
            //Destroy itself and add 1 to the coin count
            Destroy(gameObject);
            gameManager.AddCoinAmount(1);
        }
    }
}
