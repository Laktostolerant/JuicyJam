using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivation : MonoBehaviour
{
    public static Action weaponInput;
    public static Action cooldownInput;

    [SerializeField] KeyCode reloadKey;

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            weaponInput?.Invoke();
        }

        if (Input.GetKeyDown(reloadKey))
        {
            cooldownInput?.Invoke();
        }
    }
}
