using UnityEngine;
using System.Collections;

public class MobsLifecycle : MonoBehaviour, IDamagable
{
    private MobsUIController _mobsUIController;
    private EnemyAI _enemyAI;
    private bool _canDamage;
    [SerializeField] private float damageInvincibility = 1.5f;
    [SerializeField] private int health;

    private void Awake()
    {
        _canDamage = true;
        _mobsUIController = GetComponent<MobsUIController>();
        _enemyAI = GetComponent<EnemyAI>();
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
        if (health <= 0)
        {
            Death();
        }
        _mobsUIController.ReduceHealth(health);
    }

    public void Death()
    {
        _enemyAI.Death();
    }
    public IEnumerator DamageCountDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _canDamage = true;        
    }
}
