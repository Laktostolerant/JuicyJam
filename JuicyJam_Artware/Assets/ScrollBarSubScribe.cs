using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarSubScribe : MonoBehaviour
{
    [SerializeField] Scrollbar myScrollbar;
    void Start()
    {
        myScrollbar.onValueChanged.AddListener(AudioManager.Instance.GetComponent<VCAController>().SetVolume);
    }
}
