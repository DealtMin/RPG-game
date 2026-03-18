using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private WeaponSOViewer _weaplonParameters;
    private int _weaponDamage;

    private void Start()
    {
        _weaplonParameters = GetComponent<WeaponSOViewer>();
        _weaponDamage = _weaplonParameters.damage;
        if (!_weaplonParameters.isMelee)
            transform.localScale *= _weaplonParameters.initScaleFactor;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable idamagable))
        {
            idamagable.Damage(_weaponDamage);
        }
    }
}
