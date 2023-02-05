using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredCaase : MonoBehaviour, IDamageable
{
    private Rigidbody[] rigidbodyAreas;
    public static bool shatteredTheDiamondCase;
    [SerializeField] ScreenShake screenShake;
    bool hasShaked;
    [SerializeField] GameObject PickUpText;
    [SerializeField] GameObject GetOutText;
    bool hasPickedUpDiamond;
    [SerializeField] SphereCollider pickUpCollider;
    bool textTurnOff;
    bool pickUpTextActive;
    [SerializeField] GameObject diamond;
    [SerializeField] GameObject WinCollider;


    void Start()
    {
        rigidbodyAreas= GetComponentsInChildren<Rigidbody>();   
        pickUpCollider.enabled = false;
        PickUpText.SetActive(false);
        GetOutText.SetActive(false);
        WinCollider.SetActive(false);
    }

    public float HP;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && pickUpTextActive)
        {
            PickUpText.SetActive(false);
            hasPickedUpDiamond = true;
            diamond.SetActive(false);
            WinCollider.SetActive(true);
        }

        if (hasShaked)
        {
            pickUpCollider.enabled = true;
        }

        if (hasPickedUpDiamond && !textTurnOff)
        {
            textTurnOff = true; 
            GetOutText.SetActive(true);
            StartCoroutine(TextTimer());
        }

    }

    IEnumerator TextTimer()
    {
        yield return new WaitForSeconds(3);
        GetOutText.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !hasPickedUpDiamond)
        {
            PickUpText.SetActive(true);
            pickUpTextActive = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        pickUpTextActive = false;
    }

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
