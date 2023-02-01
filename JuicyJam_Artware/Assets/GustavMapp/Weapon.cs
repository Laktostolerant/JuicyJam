using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] Transform Muzzle;
    [SerializeField] string weaponModelName;

    float timeSinceLastActivation;

    GameObject WeaponProp;
    [SerializeField] GameObject MuzzleFlash;

    [SerializeField] WeaponRecoil Recoil;

    private void Start()
    {
        WeaponActivation.weaponInput += ActivateWeapon;
        WeaponActivation.cooldownInput += StartCooldown;
        weaponData.currentAmmo = weaponData.magSize;
        WeaponProp = gameObject.transform.Find(weaponModelName).gameObject;
        Recoil = transform.Find("CameraRecoil").GetComponent<WeaponRecoil>();
    }

    public void StartCooldown()
    {
        if (!weaponData.reloading && weaponData.currentAmmo != weaponData.magSize)
        {
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        weaponData.reloading = true;

        WeaponProp.SetActive(false);

        yield return new WaitForSeconds(weaponData.reloadTime);

        WeaponProp.SetActive(true);

        weaponData.currentAmmo = weaponData.magSize;

        weaponData.reloading = false;
    }

    private bool CanActivate() => !weaponData.reloading && timeSinceLastActivation > 1f/(weaponData.fireRatePerMinute / 60f);

    public void ActivateWeapon()
    {
        if (weaponData.currentAmmo > 0)
        {
            if (CanActivate())
            {
                if (Physics.Raycast(Muzzle.position, transform.forward, out RaycastHit hitInfo, weaponData.maxDistance))
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

        WeaponSway();
    }

    private void OnWeaponActivation()
    {
        GameObject Flash = Instantiate(MuzzleFlash, Muzzle);
        Destroy(Flash, 0.05f);

        Recoil.RecoilFire();
    }

    void WeaponSway()
    {
        float mouseX = Input.GetAxis("Mouse X") * weaponData.swayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * weaponData.swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(mouseX, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseY, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, weaponData.swaySmoothing * Time.deltaTime);
    }
}
