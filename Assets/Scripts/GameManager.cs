using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int m_maxTimerAmount;
    [SerializeField] private CinemachineVirtualCamera m_virtualCam;

    [SerializeField] private SpriteNumberDisplay m_liveDisplay;
    [SerializeField] private SpriteNumberDisplay m_coinDisplay;
    [SerializeField] private SpriteNumberDisplay m_timeDisplay;
    [SerializeField] private SpriteNumberDisplay m_scoreDisplay;

    [SerializeField] private GameObject m_Player;
    [SerializeField] private GameObject m_spawnPoint;

    private protected bool _inGame;

    private protected int _currentLives = 5;
    private protected int _timerAmount;
    private protected int _coinAmount = 0;
    private protected int _scoreAmount = 0;

    void Start()
    {
        GetPlayerPrefsToValues();
        GameObject player = SpawnPlayer();
        SetCinema(player);
        _timerAmount = m_maxTimerAmount;
    }   

    // Update is called once per frame
    void Update()
    {
        m_liveDisplay.DisplayNumber(_currentLives);
        m_coinDisplay.DisplayNumber(_coinAmount);
        m_timeDisplay.DisplayNumber(_timerAmount);
        m_scoreDisplay.DisplayNumber(_scoreAmount);
    }

    private GameObject SpawnPlayer()
    {
        GameObject player = Instantiate(m_Player);
        player.transform.position = m_spawnPoint.transform.position;
        player.transform.name = "Player";
        return player;
    }

    private void WritePlayerPrefsToValues()
    {
        PlayerPrefs.SetInt("Lives", this._currentLives);
        PlayerPrefs.SetInt("Coins", this._coinAmount);
        PlayerPrefs.Save();
    }

    private void GetPlayerPrefsToValues()
    {
        this._currentLives = PlayerPrefs.GetInt("Lives", 5);
        this._coinAmount = PlayerPrefs.GetInt("Coins", 0);
    }

    public CinemachineVirtualCamera getCam()
    {
        return m_virtualCam;
    }

    public void SetCinema(GameObject player)
    {
        //Freeze Camera Position
        m_virtualCam.LookAt = null;
        m_virtualCam.Follow = player.transform;
    }


    public void FreezeCinema()
    {
        //Freeze Camera Position
        m_virtualCam.LookAt = null;
        m_virtualCam.Follow = null;
    }

    //Load Game Level
    public void LoadLevel(int level)
    {
        setLevelForReset(level);
        SceneManager.LoadScene(0);
    }

    //Set and Get Protected Values

    //Coins
    public int GetCoinAmount()
    {
        return this._coinAmount;
    }

    public void SetCoinAmount(int amount)
    {
        this._coinAmount = amount;
    }

    public void AddCoinAmount(int amount)
    {
        this._coinAmount += amount;
    }

    //Lives
    public int GetLives()
    {
        return this._currentLives;
    }

    public void SetLives(int amount)
    {
        this._currentLives = amount;
    }

    public void AddLives(int amount)
    {
        this._currentLives += amount;
    }

    //Score
    public int GetScoreAmount()
    {
        return this._scoreAmount;
    }

    public void SetScoreAmount(int amount)
    {
        this._scoreAmount = amount;
    }

    public void AddScoreAmount(int amount)
    {
        this._scoreAmount += amount;
    }

    // Other

    public GameObject GetPlayer()
    {
        return GameObject.Find("Player");
    }

    public void PlayerDied()
    {
        if (_currentLives > 1)
        {
            _coinAmount = 0;
            _currentLives--;

            WritePlayerPrefsToValues();
            LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void setLevelForReset(int level)
    {
        GameObject.Find("LevelPasser").GetComponent<LevelPasser>().SetLevel(level);
    }
}
