using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Reference")]
    public Stats stats;

    [Header("Stats")]
    public float health;
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("INTERACTED");
            InteractWithObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("STOPED INTERACTED");
    }

    private void InteractWithObject()
    {

        if (this.gameObject.tag == "Damage")
        {
            if (damage != 0)
                stats.DealDamage(damage);

            Debug.Log("DAMAGE GIVEN");
        }

        if (this.gameObject.tag == "Health")
        {
            if(health!= 0)
                stats.GiveHealth(health);

            Debug.Log("HEALTH GIVEN");
        }

    }
}
