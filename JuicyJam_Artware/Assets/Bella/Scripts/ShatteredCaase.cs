using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredCaase : MonoBehaviour, IDamageable
{
    private Rigidbody[] rigidbodyAreas;
    public static bool shatteredTheDiamondCase;
    [SerializeField] ScreenShake screenShake;
    bool hasShaked;

    void Start()
    {
        rigidbodyAreas= GetComponentsInChildren<Rigidbody>();        
    }

    public float HP;

    public void Damage(float damage)
    {

        HP -= damage;

        if (HP <= 0 && !hasShaked)
        {
            for (int i = 0; i < rigidbodyAreas.Length; i++)
            {                
                rigidbodyAreas[i].GetComponent<Rigidbody>().AddExplosionForce(Random.Range(40f, 60f), 
                    -transform.forward, Random.Range(20f, 40f), Random.Range(2f, 3f), ForceMode.Impulse);

                rigidbodyAreas[i].GetComponent<Rigidbody>().drag = (Random.Range(10, 20));
                rigidbodyAreas[i].GetComponent<Rigidbody>().useGravity = true;
                rigidbodyAreas[i].GetComponent<BoxCollider>().enabled = true;
            }
            shatteredTheDiamondCase = true;
            screenShake.duration = 0.5f;
            screenShake.StartCoroutine("Shake");
            hasShaked = true;
        }
    }
}
