using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnsTestScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] AudioManager manager;
    [SerializeField] GameObject mainTheme;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            manager.Play(mainTheme);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            manager.Stop(mainTheme);
        }

    }
}
