using UnityEngine;
using System.Collections;

public class PlayerLifecycle : MonoBehaviour, IDamagable
{
    private PlayerUIController _playerUIController;
    private bool _canDamage;
    [SerializeField] private int health;
    [SerializeField] private float damageInvincibility = 2f;

    private void Awake()
    {
        _canDamage = true;
        _playerUIController = GetComponent<PlayerUIController>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_canDamage)
        {
            WeaponSOViewer weapon = other.GetComponent<WeaponSOViewer>();
            if (weapon)
            {
                _canDamage = false;
                StartCoroutine(DamageCountDown(damageInvincibility));
                Damage(weapon.damage);
            }
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            Death();
        }
        _playerUIController.ReduceHealth(health);
    }
    public IEnumerator DamageCountDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _canDamage = true;        
    }

    public void Death()
    {
        _playerUIController.Death();
    }
}
