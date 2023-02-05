using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public GameObject GameOverCanvas;

    [SerializeField] CameraController Camera;
    [SerializeField] GameObject Player;
    [SerializeField] WeaponActivation weaponActivation;
    public static bool isDead;

    private void Update()
    {
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Player_Death");
        GameOverCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Camera.enabled = false;
        Player.GetComponent<PlayerMovement>().enabled = false;
        Player.GetComponent<Dashing>().enabled = false;
        Player.GetComponent<Sliding>().enabled = false;
        weaponActivation.enabled = false;
        isDead = true;
        ShatteredCaase.shatteredTheDiamondCase = false;
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
