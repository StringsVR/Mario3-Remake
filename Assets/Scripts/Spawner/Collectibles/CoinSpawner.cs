using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _coin;
    [SerializeField] private bool destroyOnSpawn;

    // Start is called before the first frame update
    void Start()
    {
        //Create new coin from prefab
        GameObject coin = Instantiate(_coin);
        //Set position of coin to the position of spawner
        coin.transform.position = gameObject.transform.position;
        //Set coin parent to spawner
        coin.transform.SetParent(gameObject.transform);

        //Destroy itself if destroy on spawn is enabled
        if (destroyOnSpawn) Destroy(gameObject);
    }
}
