using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject resumeButton;
    public GameObject pausePanel;
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
        pausePanel.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        pausePanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
