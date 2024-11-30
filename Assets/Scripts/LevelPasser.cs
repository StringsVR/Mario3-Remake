using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPasser : MonoBehaviour
{

    private LevelPasser instance;

    private protected int _level;
    private bool hasBeenSet;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLevel(int level)
    {
        hasBeenSet = true;
        this._level = level;
    }

    public int GetLevel()
    {
        if (hasBeenSet)
        {
            return this._level;
        }
        else
        {
            return -1;
        }
    }
 
}
