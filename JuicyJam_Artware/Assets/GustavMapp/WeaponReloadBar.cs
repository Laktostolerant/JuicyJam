using UnityEngine;
using UnityEngine.UI;

public class WeaponReloadBar : MonoBehaviour
{
    GameObject Bar;
    [SerializeField] Image ProgressBar;
    [SerializeField] WeaponData weaponData;
    float remainingTime;
    float reloadTime;

    void Start()
    {
        Bar = gameObject.transform.Find("Bar").gameObject;
        reloadTime = weaponData.reloadTime;
        remainingTime = reloadTime;
        Bar.SetActive(false);
    }

    void Update()
    {
        if (remainingTime > 0 && weaponData.reloading)
        {
            Bar.SetActive(true);
            remainingTime -= Time.deltaTime;
            ProgressBar.fillAmount = remainingTime / reloadTime;
            
        }
        else
        {
            Bar.SetActive(false);
            remainingTime = reloadTime;
        }
    }
}
