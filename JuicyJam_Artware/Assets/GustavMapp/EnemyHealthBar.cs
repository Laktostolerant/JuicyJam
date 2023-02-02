using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] GameObject HealthBar;
    [SerializeField] GameObject Enemy;
    Target target;
    float HP;
    float totalHP;
    [SerializeField] Image HealthBarRemaining;
    [SerializeField] Image HealthBarBG;

    void Start()
    {
        target = Enemy.GetComponent<Target>();
        HP = target.HP;
        totalHP = HP;
    }

    void Update()
    {
        HP = target.HP;

        if (HP > 0)
        {
            HealthBar.SetActive(true);
            HealthBarRemaining.fillAmount = HP / totalHP;

        }
        else
        {
            HealthBar.SetActive(false);
        }
    }
}
