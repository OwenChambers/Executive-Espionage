using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenu;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Camera.main.gameObject.GetComponent<AudioSource>().volume /= 4;
        Time.timeScale = 0f;
        GamePaused = true;
    }
    public void ResumeGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Camera.main.gameObject.GetComponent<AudioSource>().volume *= 4;
        Time.timeScale = 1f;
        GamePaused = false;
    }
}
