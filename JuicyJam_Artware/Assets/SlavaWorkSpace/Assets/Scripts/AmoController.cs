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
    private Slider ammoSlider;
    private int currentAmmo;
    private int fullAmmo;
    [SerializeField] GameObject AmmoGameObject;
    [SerializeField] WeaponData weaponData;

    private void Start()
    {
        fullAmmo = weaponData.magSize;
    }

    // Update is called once per frame
    void Update()
    {
        currentAmmo = weaponData.currentAmmo;
        ammoSlider.value = currentAmmo;
        ammoCounter.text = currentAmmo + "/" + fullAmmo + " ";

        if (weaponData.reloading)
        {
            AmmoGameObject.SetActive(false);
        }
        else
        {
            AmmoGameObject.SetActive(true);            
        }
    }    
}
