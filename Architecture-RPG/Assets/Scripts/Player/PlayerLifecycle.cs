using System;
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
    

    public void Damage(int damage)
    {
        if (_canDamage)
        {
            health = Math.Clamp(health-damage, 0, 100);
            Debug.Log(health);
            if (health <= 0)
            {
                Death();
            }
            _canDamage = false;
            StartCoroutine(DamageCountDown(damageInvincibility));
            _playerUIController.ReduceHealth(health);
        }
    }

    
    public IEnumerator DamageCountDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _canDamage = true;        
    }

    public void Death()
    {
        _playerUIController.Death();
        Time.timeScale = 0f;
    }
}
