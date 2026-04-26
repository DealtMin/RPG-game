using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MultipleWeaponsBossController : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    private int _currWeaponIndx=0;

    public void ChooseWeapon()
    {
        _currWeaponIndx = RandomBetween(0, weapons.Length);
        weapons[_currWeaponIndx].SetActive(true);
        Debug.Log(_currWeaponIndx);
    }

    public void DeactivateWeapon()
    {
        weapons[_currWeaponIndx].SetActive(false);
    }
    
    int RandomBetween(int min, int max)
    {
        return Random.Range(min, max);
    }
}
