using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] CameraController Camera;
    [SerializeField] Slider SensX;
    [SerializeField] Slider SensY;
    [SerializeField] private GameObject optionsPanel;
    public static bool isPaused;

    private void Start()
    {
        SensX.maxValue = 10;
        SensY.maxValue = 10;
        SensX.minValue = 1;
        SensY.minValue = 1;
        SensX.value = Camera.sensX;
        SensY.value = Camera.sensY;
    }

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
        Camera.sensX = SensX.value;
        Camera.sensY = SensY.value;
    }


}
