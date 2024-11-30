using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_levelText;
    [SerializeField] protected TextMeshProUGUI m_liveText;

    private LevelPasser m_levelPasser;
    private int m_currentLevel;
    private int m_liveAmount;

    // Start is called before the first frame update
    void Start()
    {
        m_liveAmount = PlayerPrefs.GetInt("Lives", 5);

        GameObject levelPasser;
        try
        {
            m_currentLevel = 1;

            levelPasser = GameObject.Find("LevelPasser");
            if (levelPasser != null)
            {
                m_currentLevel = levelPasser.GetComponent<LevelPasser>().GetLevel();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        m_levelText.text = $"Level: {m_currentLevel.ToString()}";
        m_liveText.text = $"Lives: {m_liveAmount}";
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        // Logic to apply the quarantine effect
        Debug.Log($"Waiting 5s for level {m_currentLevel}");
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(m_currentLevel);
    }


}
