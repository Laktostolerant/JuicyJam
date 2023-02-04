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

        if(sceneNumber == 1)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Death_Restart");
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        }

        SceneManager.LoadScene(sceneNumber);
    }
    public void ExitFromGame()
    {
        Application.Quit();

    }







}
