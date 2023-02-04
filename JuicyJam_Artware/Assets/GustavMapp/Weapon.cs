using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] Transform Muzzle;
    [SerializeField] Camera cam;
    [SerializeField] string weaponModelName;

    float timeSinceLastActivation;

    GameObject WeaponProp;
    [SerializeField] GameObject MuzzleFlash;
    [SerializeField] GameObject BulletHole;

    [SerializeField] WeaponRecoil Recoil;

    private void Start()
    {
        WeaponActivation.weaponInput += ActivateWeapon;
        WeaponActivation.cooldownInput += StartCooldown;
        weaponData.currentAmmo = weaponData.magSize;
        WeaponProp = gameObject.transform.Find(weaponModelName).gameObject;
        weaponData.reloading = false;
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
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Gun_Reload", gameObject);
        weaponData.reloading = true;

        WeaponProp.SetActive(false);

        yield return new WaitForSeconds(weaponData.reloadTime);

        WeaponProp.SetActive(true);

        weaponData.currentAmmo = weaponData.magSize;

        weaponData.reloading = false;
    }

    private bool CanActivate() => !weaponData.reloading && timeSinceLastActivation > 1f / (weaponData.fireRatePerMinute / 60f);

    public void ActivateWeapon()
    {
        if (weaponData.currentAmmo > 0)
        {
            if (CanActivate())
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Gun_Shot", gameObject);

                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, weaponData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(weaponData.damage);

                    GameObject obj = Instantiate(BulletHole, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    obj.transform.position += obj.transform.position / -1000;
                    Destroy(obj, 1f);
                }

                weaponData.currentAmmo--;
                timeSinceLastActivation = 0;
                OnWeaponActivation();
            }
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Gun_Empty", gameObject);
        }
    }

    private void Update()
    {
        timeSinceLastActivation += Time.deltaTime;

        //WeaponSway();
    }

    private void OnWeaponActivation()
    {
        GameObject Flash = Instantiate(MuzzleFlash, Muzzle);
        Destroy(Flash, 0.03f);

        Recoil.RecoilFire();
    }

    void WeaponSway()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * weaponData.swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * weaponData.swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, weaponData.swaySmoothing * Time.deltaTime);
    }

    private void OnDestroy()
    {
        WeaponActivation.weaponInput -= ActivateWeapon;
        WeaponActivation.cooldownInput -= StartCooldown;
    }
}
