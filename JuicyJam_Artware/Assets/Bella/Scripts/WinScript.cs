using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    [SerializeField] GameObject winCanvas;
    [SerializeField] CameraController Camera;
    [SerializeField] GameObject Player;
    [SerializeField] WeaponActivation weaponActivation;

    private void Start()
    {
        winCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Camera.enabled = false;
            Player.GetComponent<PlayerMovement>().enabled = false;
            Player.GetComponent<Dashing>().enabled = false;
            Player.GetComponent<Sliding>().enabled = false;
            weaponActivation.enabled = false;
            ShatteredCaase.shatteredTheDiamondCase = false;
        }
    }
}
