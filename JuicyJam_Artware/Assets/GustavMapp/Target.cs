using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float HP;
    [SerializeField] bool vitalArea;
    [SerializeField] float damageMultiplier;

    public void Damage(float damage)
    {
        if (vitalArea)
        {
            damage *= damageMultiplier;
        }

        HP -= damage;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
