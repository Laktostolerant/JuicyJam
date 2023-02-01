using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Gun")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")] 
    public new string name;

    [Header("Shooting Stats")]
    public float damage;
    public float maxDistance;
    public bool isSemiAutomatic;

    [Header("Weapon Sway")]
    public float swaySmoothing;
    public float swayMultiplier;

    [Header("Reloading Stats")]
    public int currentAmmo;
    public int magSize;
    public float fireRatePerMinute;
    public float reloadTime;
    [HideInInspector]
    public bool reloading;
}
