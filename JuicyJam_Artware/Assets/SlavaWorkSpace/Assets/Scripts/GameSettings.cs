using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{

    [SerializeField]
    private GameObject optionsPanel;
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void CloseButton()
    {
        optionsPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
