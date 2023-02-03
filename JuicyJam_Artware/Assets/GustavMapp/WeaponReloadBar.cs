using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponReloadBar : MonoBehaviour
{
    GameObject Bar;
    GameObject AmmoCounter;
    [SerializeField] TextMeshProUGUI AmmoCount;
    [SerializeField] TextMeshProUGUI MaxAmmo;
    [SerializeField] Image ProgressBar;
    [SerializeField] WeaponData weaponData;

    float remainingTime;
    float reloadTime;

    int ammoRemaining;

    void Start()
    {
        Bar = gameObject.transform.Find("Bar").gameObject;
        AmmoCounter = gameObject.transform.Find("AmmoCounter").gameObject;
        reloadTime = weaponData.reloadTime;
        remainingTime = reloadTime;
        Bar.SetActive(false);
        AmmoCounter.SetActive(true);
        MaxAmmo.text = weaponData.magSize.ToString();
    }

    void Update()
    {
        ammoRemaining = weaponData.currentAmmo;

        AmmoCount.text = ammoRemaining.ToString();

        if (remainingTime > 0 && weaponData.reloading)
        {
            AmmoCounter.SetActive(false);
            Bar.SetActive(true);
            remainingTime -= Time.deltaTime;
            ProgressBar.fillAmount = remainingTime / reloadTime;            
        }
        else
        {
            AmmoCounter.SetActive(true);
            Bar.SetActive(false);
            remainingTime = reloadTime;
        }
    }
}
