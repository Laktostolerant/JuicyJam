using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameObject currentSound;

    public void Play(GameObject musicToActivate)
    {
        currentSound.SetActive(false);
        currentSound = musicToActivate;
        currentSound.SetActive(true);
    }

    public void Stop()
    {
        currentSound.SetActive(false);
    }
}
