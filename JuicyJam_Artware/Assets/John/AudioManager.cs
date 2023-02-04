using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] public GameObject MainTheme;
    [SerializeField] public GameObject PianoTheme;
    [SerializeField] public GameObject ChaosTheme;
    [SerializeField] public GameObject EQSnapShot;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Play(MainTheme);
    }
    public void Play(GameObject musicToActivate)
    {
        musicToActivate.SetActive(true);
    }

    public void Stop(GameObject musicToDeactivate)
    {
        musicToDeactivate.SetActive(false);
    }

    public void PlayUIClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
    }

    public void PlayUIHover()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Hover");
    }

    public void PlayRestart()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Death_Restart");
    }
}
