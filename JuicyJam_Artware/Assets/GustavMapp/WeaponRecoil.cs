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

    float snapback;
    float returnSpeed;

    [SerializeField] WeaponData weaponData;

    void Start()
    {
        recoilX = -weaponData.recoilX;
        recoilY = weaponData.recoilY;
        recoilZ = weaponData.recoilZ;
        snapback = weaponData.snapback;
        returnSpeed = weaponData.returnSpeed;
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snapback * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
