using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Stats")]
    public float health;

    private void Update()
    {
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("YOU DIED");
    }

    public void DealDamage(float damage)
    {
        health -= damage;
    }

    public void GiveHealth(float healthAmount)
    {
        health += healthAmount;
    }
}
