using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCAController : MonoBehaviour
{
    // Start is called before the first frame update
    private FMOD.Studio.VCA vca;
    
    void Start()
    {
        vca = FMODUnity.RuntimeManager.GetVCA("vca:/Master");
    }

    public void SetVolume(float volume)
    {
        vca.setVolume(volume);
    }
}
