using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
   public void ChangeScene()
    {
        SceneManager.LoadScene("Owen Scene");
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game quit successfully");
    }
}
