using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredCaase : MonoBehaviour, IDamageable
{
    private Rigidbody[] rigidbodyAreas;
    public static bool shatteredTheDiamondCase;

    void Start()
    {
        rigidbodyAreas= GetComponentsInChildren<Rigidbody>();        
    }

    public float HP;

    public void Damage(float damage)
    {

        HP -= damage;

        if (HP <= 0)
        {
            for (int i = 0; i < rigidbodyAreas.Length; i++)
            {
                rigidbodyAreas[i].GetComponent<Rigidbody>().useGravity = true;
            }
            shatteredTheDiamondCase = true;
        }
    }


}
