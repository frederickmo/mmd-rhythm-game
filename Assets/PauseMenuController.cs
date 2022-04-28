using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private static bool _isGamePaused;

    public GameObject pauseMenuUI;

    public AudioSource bgm;

    private void Start()
    {
        _isGamePaused = false;
    }

    public void PauseButtonHandler()
    {
        if (_isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void BackToStartMenuHandler()
    {
        // SceneManager.LoadScene("StartScene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        // SceneManager.
    }

    private void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        bgm.UnPause();
        _isGamePaused = false;
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        bgm.Pause();
        _isGamePaused = true;
    }
}
