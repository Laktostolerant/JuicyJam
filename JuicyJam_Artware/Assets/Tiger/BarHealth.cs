using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BarHealth : MonoBehaviour, IDamageable
{
    [SerializeField] float HP;

    Collider collider;
    [SerializeField] GameObject wholeBars;
    [SerializeField] GameObject brokenBars;
    [SerializeField] ScreenShake screenShake;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    public void Damage(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            DestroyBars();
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Hit", gameObject);
        }
    }

    void DestroyBars()
    {
        screenShake.start = true;
        collider.enabled = !collider.enabled;
        wholeBars.SetActive(false);
        brokenBars.SetActive(true);
        this.enabled = false;
    }
}
