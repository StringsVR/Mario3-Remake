using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _goomba;

    // Start is called before the first frame update
    void Start()
    {
        GameObject goomba = Instantiate(_goomba);
        goomba.transform.position = gameObject.transform.position;
        goomba.transform.SetParent(gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
