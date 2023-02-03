using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderActivator : MonoBehaviour
{
    bool activateWeapons;
    GameObject WeaponHolder;

    void Start()
    {
        WeaponHolder = gameObject.transform.Find("WeaponHolder").gameObject;
    }

    void Update()
    {
        activateWeapons = ShatteredCaase.shatteredTheDiamondCase;
        if (!activateWeapons)
        {
            WeaponHolder.SetActive(false);
        }

        if (activateWeapons)
        {
            WeaponHolder.SetActive(true);
        }
    }
}
