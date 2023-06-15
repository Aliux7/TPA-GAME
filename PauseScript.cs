using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseScreen, PlayerStats;
    public CinemachineBrain brain;
    public CinemachineVirtualCamera cutScene;


    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerStats.SetActive(false);
            Invoke("offCutScene", 8.5f);    
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            if (GameIsPaused)
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
        Cursor.lockState = CursorLockMode.Locked;
        brain.enabled = true;
        PauseScreen.SetActive(false);
        PlayerStats.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        brain.enabled = false;
        PauseScreen.SetActive(true);
        PlayerStats.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void offCutScene()
    {
        cutScene.Priority = 8;
        PlayerStats.SetActive(true);
    }
}
