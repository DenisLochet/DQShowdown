using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void QuitGameBtn()
    {
        Application.Quit();
    }

    public void PlayBtn()
    {
        SceneManager.LoadScene(1);
    }

    public void TeamBuildBtn()
    {
        SceneManager.LoadScene(2);
    }
}
