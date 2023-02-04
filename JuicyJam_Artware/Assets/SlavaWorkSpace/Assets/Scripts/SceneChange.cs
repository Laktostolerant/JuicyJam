using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] CameraController Camera;
    public void ChangeScene(int sceneNumber)
    {
        Time.timeScale = 1;
        Camera.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(sceneNumber);
    }
    public void ExitFromGame()
    {
        Application.Quit();

    }







}
