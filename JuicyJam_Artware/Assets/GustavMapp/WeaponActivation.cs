using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivation : MonoBehaviour
{
    public static Action weaponInput;
    public static Action cooldownInput;
    public static Action meleeInput;

    [SerializeField] WeaponData weaponData;
    [SerializeField] KeyCode reloadKey = KeyCode.R;
    [SerializeField] KeyCode meleeKey = KeyCode.V;

    void Update()
    {
        if (!GameSettings.isPaused)
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

            if (Input.GetKeyDown(reloadKey))
            {
                cooldownInput?.Invoke();
            }

            if (Input.GetKeyDown(meleeKey))
            {
                meleeInput?.Invoke();
            }
        }
        
    }
}
