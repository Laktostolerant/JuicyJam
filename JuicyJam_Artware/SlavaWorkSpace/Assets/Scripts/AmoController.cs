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
    private Slider amoBar;
    private int currentAmo = 30;
    private int fullAmo = 90;


    

    // Update is called once per frame
    void Update()
    {
        amoBar.value = fullAmo;
        if (Input.GetKeyDown(KeyCode.T) )
        {
            fullAmo += 30;
           
        }
       
        if (Input.GetKeyDown(KeyCode.M) && currentAmo!=0)
        {
            currentAmo -= 10;

        }
        amoCounter.text = currentAmo + "/" + fullAmo + " ";
        


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
