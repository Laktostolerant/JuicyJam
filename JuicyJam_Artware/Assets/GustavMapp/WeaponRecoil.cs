using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    Vector3 currentRotation;
    Vector3 targetRotation;

    float recoilXP;
    float recoilYP;
    float recoilZP;

    float recoilXR;
    float recoilYR;
    float recoilZR;

    float recoilXM;
    float recoilYM;
    float recoilZM;

    float snapbackP;
    float returnSpeedP;

    float snapbackR;
    float returnSpeedR;

    [SerializeField] WeaponData weaponDataPistol;
    [SerializeField] WeaponData weaponDataRifle;
    [SerializeField] WeaponData meleeData;

    void Start()
    {
        recoilXP = -weaponDataPistol.recoilX;
        recoilYP = weaponDataPistol.recoilY;
        recoilZP = weaponDataPistol.recoilZ;
        snapbackP = weaponDataPistol.snapback;
        returnSpeedP = weaponDataPistol.returnSpeed;

        recoilXR = -weaponDataRifle.recoilX;
        recoilYR = weaponDataRifle.recoilY;
        recoilZR = weaponDataRifle.recoilZ;
        snapbackR = weaponDataRifle.snapback;
        returnSpeedR = weaponDataRifle.returnSpeed;

        recoilXM = -meleeData.recoilX;
        recoilYM = meleeData.recoilY;
        recoilZM = meleeData.recoilZ;
    }

    void Update()
    {
        RecoilManager();
    }

    public void RecoilPistol()
    {
        targetRotation += new Vector3(recoilXP, Random.Range(-recoilYP, recoilYP), Random.Range(-recoilZP, recoilZP));
    }

    public void RecoilRiffle()
    {
        targetRotation += new Vector3(recoilXR, Random.Range(-recoilYR, recoilYR), Random.Range(-recoilZR, recoilZR));
    }

    void RecoilManager()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeedP * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snapbackP * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilMelee()
    {
        targetRotation += new Vector3(recoilXM, Random.Range(-recoilYM, recoilYM), Random.Range(-recoilZM, recoilZM));
    }
}
