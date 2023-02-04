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
        SceneManager.LoadSceneAsync(sceneNumber);
        Time.timeScale = 1;
        Camera.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ExitFromGame()
    {
        Application.Quit();

    }







}
