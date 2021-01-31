using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_endScreenRoot = null;

    [SerializeField]
    private TMPro.TMP_Text m_scoreTextUI;

    [SerializeField]
    private string m_scoreText = "Score: {0}";

    bool m_gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        m_endScreenRoot.SetActive(false);
        m_gameStarted = true;
        GameController.Instance().OnLevelEnd += OnLevelEnd;
    }

    private void OnLevelEnd()
    {
        m_endScreenRoot.SetActive(true);
        m_scoreTextUI.text = string.Format(m_scoreText, GameController.Instance().LevelScore);
        m_gameStarted = false;
    }

    private void Update()
    {
        if (m_gameStarted == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
