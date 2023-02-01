using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivation : MonoBehaviour
{
    public static Action weaponInput;
    public static Action cooldownInput;

    [SerializeField] WeaponData weaponData;

    void Update()
    {
        if (weaponData.isSemiAutomatic)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weaponInput?.Invoke();
            }
        }

        if (!weaponData.isSemiAutomatic)
        {
            if (Input.GetButton("Fire1"))
            {
                weaponInput?.Invoke();
            }            
        }

        if (Input.GetButtonDown("Fire2"))
        {
            cooldownInput?.Invoke();
        }
    }
}
