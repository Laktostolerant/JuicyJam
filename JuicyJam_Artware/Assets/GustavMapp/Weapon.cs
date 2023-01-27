using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] Transform muzzleFlash;

    float timeSinceLastActivation;

    private void Start()
    {
        WeaponActivation.weaponInput += ActivateWeapon;
        WeaponActivation.cooldownInput += StartCooldown;
    }

    public void StartCooldown()
    {
        if (!weaponData.reloading)
        {
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        weaponData.reloading = true;

        Debug.Log("Reloading...");

        yield return new WaitForSeconds(weaponData.reloadTime);

        Debug.Log("Reloaded!");

        weaponData.currentAmmo = weaponData.magSize;

        weaponData.reloading = false;
    }

    private bool CanActivate() => !weaponData.reloading && timeSinceLastActivation > 1f/(weaponData.fireRate / 60f);

    public void ActivateWeapon()
    {
        if (weaponData.currentAmmo > 0)
        {
            if (CanActivate())
            {
                if (Physics.Raycast(muzzleFlash.position, transform.forward, out RaycastHit hitInfo, weaponData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(weaponData.damage);
                    Debug.Log(hitInfo.transform.name);
                }

                weaponData.currentAmmo--;
                timeSinceLastActivation = 0;
                OnWeaponActivation();
            }
        }
    }

    private void Update()
    {
        timeSinceLastActivation += Time.deltaTime;

        Debug.DrawRay(muzzleFlash.position, muzzleFlash.forward, Color.green);
    }

    private void OnWeaponActivation()
    {
        throw new NotImplementedException();
    }
}
