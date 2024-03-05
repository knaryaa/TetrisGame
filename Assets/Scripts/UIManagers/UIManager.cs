using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool isGameStopped = false;

    public GameObject pausePanel;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        if (pausePanel)
        {
            pausePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanelOnOff();
        }
    }

    public void PausePanelOnOff()
    {
        if (gameManager.gameOver)
        {
            return;
        }

        isGameStopped = !isGameStopped;

        if (pausePanel)
        {
            pausePanel.SetActive(isGameStopped);

            if (SoundManager.instance)
            {
                SoundManager.instance.PlaySoundEffect(0);
                Time.timeScale = (isGameStopped) ? 0 : 1;
            }
        }
    }

    public void PlayAgainFNC()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
