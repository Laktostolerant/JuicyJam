using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public GameObject GameOverCanvas;

    [SerializeField] CameraController Camera;

    private void Update()
    {
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("YOU DIED");

        // TODO - ADD WHAT HAPPENS WHEN DEAD
        // STOP TIME
        GameOverCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        Camera.enabled = false;
        // SOMETHING MORE?
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
