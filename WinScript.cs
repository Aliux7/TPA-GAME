using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    public static bool GameWin = false;
    public GameObject WinScreen, PlayerStats;
    public CinemachineBrain brain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameWin == true)
        {
            if (WinScreen.activeSelf == false)
            {
                Cursor.lockState = CursorLockMode.None;
                brain.enabled = false;
                WinScreen.SetActive(true);
                PlayerStats.SetActive(false);
                Time.timeScale = 0f;
            }
        }
    }

    public void village()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        WinScreen.SetActive(false);
        GameWin = false;
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        WinScreen.SetActive(false);
        GameWin = false;
    }
}
