using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmoController : MonoBehaviour
{
    [SerializeField]
    private Text ammoCounter;
    [SerializeField]
    private Slider ammoBar;
    private int currentAmmo;
    private int fullAmmo;
    GameObject AmmoSlider;
    [SerializeField] WeaponData weaponData;

    private void Start()
    {
        AmmoSlider = gameObject.transform.Find("Ammo").gameObject;
        fullAmmo = weaponData.magSize;
    }

    // Update is called once per frame
    void Update()
    {
        currentAmmo = weaponData.currentAmmo;
        ammoBar.value = currentAmmo;
        if (Input.GetKeyDown(KeyCode.T) )
        {
            fullAmmo += 30;
           
        }
       
        if (Input.GetKeyDown(KeyCode.M) && currentAmmo!=0)
        {
            currentAmmo -= 10;

        }
        ammoCounter.text = currentAmmo + "/" + fullAmmo + " ";
    }
    
}
