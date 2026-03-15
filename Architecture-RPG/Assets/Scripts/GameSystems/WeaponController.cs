using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private WeaponSOViewer _weaplonParameters;
    private int _weaponDamage;

    private void Awake()
    {
        _weaplonParameters = GetComponent<WeaponSOViewer>();
        _weaponDamage = _weaplonParameters.damage;
        transform.localScale *= _weaplonParameters.initScaleFactor;

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(11111);
        if (other.gameObject.TryGetComponent(out IDamagable idamagable))
        {
            Debug.Log(22222);
            idamagable.Damage(_weaponDamage);

        }
    }
}
