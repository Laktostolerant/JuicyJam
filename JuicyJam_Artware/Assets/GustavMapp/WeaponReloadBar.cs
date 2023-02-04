using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponReloadBar : MonoBehaviour
{
    [SerializeField] GameObject Bar;
    GameObject AmmoCounter;
    //[SerializeField] TextMeshProUGUI AmmoCount;
    //[SerializeField] TextMeshProUGUI MaxAmmo;
    [SerializeField] Image ProgressBar;
    [SerializeField] WeaponData weaponData;

    float remainingTime;
    float reloadTime;

    int ammoRemaining;

    void Start()
    {
        reloadTime = weaponData.reloadTime;
        remainingTime = reloadTime;
    }

    void Update()
    {

        //AmmoCount.text = ammoRemaining.ToString();

        if (weaponData.reloading)
        {
            //AmmoCounter.SetActive(false);
            Bar.SetActive(true);
            remainingTime -= Time.deltaTime;
            ProgressBar.fillAmount = remainingTime / reloadTime;            
        }
        else
        {
            //AmmoCounter.SetActive(true);
            Bar.SetActive(false);
            remainingTime = reloadTime;
        }
        
    }
}
