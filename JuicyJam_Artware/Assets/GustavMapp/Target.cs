using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float HP;
    [SerializeField] bool player;
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
            if(!player)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Cyborg/Cyborg_Death", gameObject);
            }

            Destroy(gameObject);
        }
        else
        {
            if (player)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Player_Hit", gameObject);
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Cyborg/Cyborg_Hit", gameObject);
            }
        }
    }
}
