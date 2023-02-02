using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponReloadBar : MonoBehaviour
{
    GameObject Bar;
    GameObject AmmoGameObject;
    [SerializeField] Image ProgressBar;
    [SerializeField] WeaponData weaponData;
    [SerializeField] TextMeshProUGUI ammoCounter;
    [SerializeField] TextMeshProUGUI ammoReserve;
    int ammo;
    int reserveAmmo;
    float remainingTime;
    float reloadTime;

    void Start()
    {
        AmmoGameObject = gameObject.transform.Find("Ammo").gameObject;
        Bar = gameObject.transform.Find("Bar").gameObject;
        reloadTime = weaponData.reloadTime;
        remainingTime = reloadTime;
        Bar.SetActive(false);
        reserveAmmo = weaponData.magSize;
    }

    void Update()
    {
        ammo = weaponData.currentAmmo;

        ammoCounter.text = ammo.ToString();

        ammoReserve.text = reserveAmmo.ToString();

        if (remainingTime > 0 && weaponData.reloading)
        {
            AmmoGameObject.SetActive(false);
            Bar.SetActive(true);
            remainingTime -= Time.deltaTime;
            ProgressBar.fillAmount = remainingTime / reloadTime;
            
        }
        else
        {
            AmmoGameObject.SetActive(true);
            Bar.SetActive(false);
            remainingTime = reloadTime;
        }
    }
}
