using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int selectedWeapon = 0;
    public GameObject player;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int lastSelectedWeapon = selectedWeapon;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }

        if (lastSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                player.GetComponent<HeroController>().Gun = weapon.GameObject();
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    public void SpawnWeapon(GameObject weaponItem, int slot)
    {
        
        Instantiate(weaponItem.gameObject, transform).transform.SetSiblingIndex(slot);
    }

    public void DestroyWeapon(GameObject weaponItem)
    {
        foreach (Transform weapon in transform)
        {
            if (weapon.gameObject.GetComponent<GunController>().weaponName == weaponItem.gameObject.GetComponent<GunController>().weaponName)
            {
                Destroy(weapon.gameObject);
            }
        }
    }
}
