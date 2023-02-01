using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmoController : MonoBehaviour
{
    [SerializeField]
    private Text amoCounter;
    [SerializeField]
    private Image amoBar;
    private int currentAmo = 30;
    private int fullAmo = 90;
    public float fullBar=1f;

    

    // Update is called once per frame
    void Update()
    {

        
       
        if (Input.GetKeyDown(KeyCode.M) && currentAmo!=0)
        {
            currentAmo -= 10;

        }
        amoCounter.text = currentAmo + "/" + fullAmo;
        amoBar.fillAmount = fullBar;
        fullBar = fullAmo;


        if (Input.GetKeyDown(KeyCode.R) || currentAmo == 0)
        {
            
            Reload();
            


        }

    }

    public void Reload()
    {
        int reason = 30 - currentAmo;

        if (fullAmo >= reason)
        {
            fullAmo = fullAmo - reason;
           
            currentAmo = 30;
        }
        else
        {

            currentAmo = currentAmo + fullAmo;
            fullAmo = 0;
        }

      


    }

    
}
