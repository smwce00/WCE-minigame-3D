using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
    public void SceneChangetoN_AIM()
    {
        SceneManager.LoadScene("FPS_NormalAIM");

    }
    public void SceneChangetoN_Wall()
    {
        SceneManager.LoadScene("FPS_NormalWall");

    }
    public void SceneChangetoAimHack()
    {
        SceneManager.LoadScene("FPS_AIM");

    }

    public void SceneChangetoWallHack()
    {
        SceneManager.LoadScene("FPS_Wall");

    }

    public void SceneChangetoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }
}
