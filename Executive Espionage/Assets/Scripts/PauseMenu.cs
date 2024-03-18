using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenu;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

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

    public void SaveGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        PlayerPrefs.SetFloat("PlayerPosX", playerTransform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerTransform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerTransform.position.z);
        PlayerPrefs.SetFloat("PlayerRotX", playerTransform.rotation.eulerAngles.x);
        PlayerPrefs.SetFloat("PlayerRotY", playerTransform.rotation.eulerAngles.y);
        PlayerPrefs.SetFloat("PlayerRotZ", playerTransform.rotation.eulerAngles.z);

        Debug.Log("Game saved.");
    }

    public void LoadGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        float posX = PlayerPrefs.GetFloat("PlayerPosX");
        float posY = PlayerPrefs.GetFloat("PlayerPosY");
        float posZ = PlayerPrefs.GetFloat("PlayerPosZ");
        float rotX = PlayerPrefs.GetFloat("PlayerRotX");
        float rotY = PlayerPrefs.GetFloat("PlayerRotY");
        float rotZ = PlayerPrefs.GetFloat("PlayerRotZ");

        Vector3 position = new Vector3(posX, posY, posZ);
        Quaternion rotation = Quaternion.Euler(rotX, rotY, rotZ);

        playerTransform.position = position;
        playerTransform.rotation = rotation;

        Debug.Log("Game loaded.");
    }
}
