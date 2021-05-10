using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void ExitButton() {
        Application.Quit();
        Debug.Log("Game Closed");
    }
    public void StartGame1() {
        SceneManager.LoadScene("FPS_NormalAIM");

    }
    public void StartGame2()
    {
        SceneManager.LoadScene("FPS_NormalWall");

    }

}
