using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] WeaponData melee;
    [SerializeField] WeaponRecoil Recoil;
    [SerializeField] Camera cam;
    [SerializeField] GameObject Baton;
    [SerializeField] Animator SwingAnimation;
    float timeSinceLastActivation;

    void Start()
    {
        WeaponActivation.meleeInput += ActivateMelee;
        cam = Camera.main;
        Baton.SetActive(false);
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
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Bat_Hit", gameObject);

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, melee.maxDistance))
            {
                IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                damageable?.Damage(melee.damage);
                Recoil.RecoilMelee();
            }
            timeSinceLastActivation = 0;
            StartCoroutine(Swing());
        }
    }

    private void OnDestroy()
    {
        WeaponActivation.meleeInput -= ActivateMelee;
    }

    IEnumerator Swing()
    {
        Baton.SetActive(true);
        SwingAnimation.Play("Swing");
        yield return new WaitForSeconds(1f);
        Baton.SetActive(false);
    }
}
