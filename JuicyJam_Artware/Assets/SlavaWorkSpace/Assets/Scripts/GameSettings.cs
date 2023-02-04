using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] CameraController Camera;
    [SerializeField]
    private GameObject optionsPanel;
    bool isPaused;
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            optionsPanel.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Camera.enabled = false;
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            CloseButton();
        }
    }

    public void CloseButton()
    {

        optionsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera.enabled = true;
        Time.timeScale = 1;
        isPaused = false;
    }
}
