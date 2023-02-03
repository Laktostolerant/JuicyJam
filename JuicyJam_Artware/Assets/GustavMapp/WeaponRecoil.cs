using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    Vector3 currentRotation;
    Vector3 targetRotation;

    float recoilX;
    float recoilY;
    float recoilZ;

    float recoilXM;
    float recoilYM;
    float recoilZM;

    float snapback;
    float returnSpeed;

    [SerializeField] WeaponData weaponData;
    [SerializeField] WeaponData meleeData;

    void Start()
    {
        recoilX = -weaponData.recoilX;
        recoilY = weaponData.recoilY;
        recoilZ = weaponData.recoilZ;
        snapback = weaponData.snapback;
        returnSpeed = weaponData.returnSpeed;

        recoilXM = -meleeData.recoilX;
        recoilYM = meleeData.recoilY;
        recoilZM = meleeData.recoilZ;
    }

    void Update()
    {
        RecoilManager();
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    void RecoilManager()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snapback * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilMelee()
    {
        targetRotation += new Vector3(recoilXM, Random.Range(-recoilYM, recoilYM), Random.Range(-recoilZM, recoilZM));
    }
}
