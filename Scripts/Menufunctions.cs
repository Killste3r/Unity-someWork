using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menufunctions : MonoBehaviour
{ 

    public void NewGame()
    {
            SceneManager.LoadScene("starting level");
    }
    public void LoadGame()
    {

    }
    public void Options()
    {

    }
    public void exit()
    {
            Application.Quit();
    }

}
