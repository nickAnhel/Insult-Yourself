using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    private GameObject player;
    private GameObject dontDestroy;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        player.SetActive(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        player.SetActive(false);
    }

    public void LoadMenu()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        Destroy(dontDestroy);
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        gameIsPaused = false;
        Application.Quit();
    }
}
