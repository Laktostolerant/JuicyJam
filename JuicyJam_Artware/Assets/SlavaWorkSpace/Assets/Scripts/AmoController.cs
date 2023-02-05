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
    [SerializeField] int matchingNumber;

    private void Start()
    {
        fullAmmo = weaponData.magSize;
    }

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

        if (matchingNumber == WeaponSwap.selectedWeapon)
        {
            AmmoGameObject.SetActive(true);
        }

        if (matchingNumber != WeaponSwap.selectedWeapon)
        {
            AmmoGameObject.SetActive(false);
        }
    }    
}
