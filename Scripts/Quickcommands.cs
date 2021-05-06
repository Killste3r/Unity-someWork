using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quickcommands : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject PauseMenuUI;

    void Update()
    {

        resetscene();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                else
                {
                    Time.timeScale = 0;
                }
        }
    }
    void resetscene()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("Demo");
        }
    }
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        GameIsPause = false;
    }
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        GameIsPause = true;
    }
    public void ExitToMenu()
    {
            SceneManager.LoadScene("MainMenu");
    }
}
