using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] WeaponData melee;
    [SerializeField] WeaponRecoil Recoil;
    Camera cam;
    float timeSinceLastActivation;

    void Start()
    {
        WeaponActivation.meleeInput += ActivateMelee;
        cam = GetComponent<Camera>();
    }

    private bool CanMelee() => timeSinceLastActivation > 1f / (melee.fireRatePerMinute / 60f);

    void Update()
    {
        timeSinceLastActivation += Time.deltaTime;
    }

    public void ActivateMelee()
    {
        if (CanMelee())
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, melee.maxDistance))
            {
                IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                damageable?.Damage(melee.damage);
                timeSinceLastActivation = 0;
                Recoil.RecoilMelee();
            }
        }
    }
}
