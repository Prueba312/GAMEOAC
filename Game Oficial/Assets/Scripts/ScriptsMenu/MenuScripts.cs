using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    public void OnplayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void OnMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnquitButton()
    {
        Application.Quit();
    }
}
