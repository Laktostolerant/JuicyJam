using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponShoot : MonoBehaviour
{
    [SerializeField] WeaponData AiWeaponData;
    [SerializeField] Transform Muzzle;
    [SerializeField] Transform BulletSpawner;

    float timeSinceLastActivation;

    [SerializeField] GameObject MuzzleFlash;
    [SerializeField] GameObject BulletHole;

    private void Start()
    {
        AIWeaponActivation.AIweaponInput += AIActivateWeapon;
        AiWeaponData.currentAmmo = AiWeaponData.magSize;
        AiWeaponData.reloading = false;
    }

    public void StartCooldown()
    {
        if (!AiWeaponData.reloading && AiWeaponData.currentAmmo == 0)
        {
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        AiWeaponData.reloading = true;

        yield return new WaitForSeconds(AiWeaponData.reloadTime);

        AiWeaponData.currentAmmo = AiWeaponData.magSize;

        AiWeaponData.reloading = false;
    }

    private bool CanActivate() => !AiWeaponData.reloading && timeSinceLastActivation > 1f / (AiWeaponData.fireRatePerMinute / 60f);

    public void AIActivateWeapon()
    {
        if (AiWeaponData.currentAmmo > 0)
        {
            if (CanActivate())
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Cyborg/Cyborg_Gun_Shot", gameObject);

                if (Physics.Raycast(BulletSpawner.transform.position, BulletSpawner.transform.forward, out RaycastHit hitInfo, AiWeaponData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(AiWeaponData.damage);

                    GameObject obj = Instantiate(BulletHole, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    obj.transform.position += obj.transform.position / -1000;
                    Destroy(obj, 1f);
                }

                AiWeaponData.currentAmmo--;
                timeSinceLastActivation = 0;
                OnWeaponActivation();
            }
        }
        else
        {
            if (CanActivate())
            {
                timeSinceLastActivation = 0;
            }
        }
    }

    private void Update()
    {
        timeSinceLastActivation += Time.deltaTime;

        StartCooldown();
    }

    private void OnWeaponActivation()
    {
        GameObject Flash = Instantiate(MuzzleFlash, Muzzle);
        Destroy(Flash, 0.03f);
    }

    private void OnDestroy()
    {
        AIWeaponActivation.AIweaponInput -= AIActivateWeapon;
    }
}
