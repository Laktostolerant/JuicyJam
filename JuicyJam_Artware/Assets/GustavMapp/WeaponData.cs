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

    [Header("Recoil Stats")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float snapback;
    public float returnSpeed;

    [Header("Weapon Sway Stats")]
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
