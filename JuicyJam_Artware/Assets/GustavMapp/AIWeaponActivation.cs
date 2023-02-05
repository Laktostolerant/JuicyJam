using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponActivation : MonoBehaviour
{
    public static Action AIweaponInput;

    [SerializeField] WeaponData weaponData;
    [SerializeField] Transform PlayerSpotter;
    [SerializeField] float triggerRange;

    void Update()
    {
        if (!GameSettings.isPaused)
        {            
            if (Physics.Raycast(PlayerSpotter.position, PlayerSpotter.forward, out RaycastHit hitInfo, triggerRange))
            {
                AIweaponInput?.Invoke();
                Debug.Log("Hit");
            }            
        }

        Debug.DrawRay(PlayerSpotter.position, PlayerSpotter.forward, Color.green);
    }
}
