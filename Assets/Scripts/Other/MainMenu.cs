using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewDesign()
    {
        SceneManager.LoadScene("Design");
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Help()
    {
        SceneManager.LoadScene("Help");
    }


    public void OnApplicationQuit()
    {
        
    }
}
