using UnityEngine;

public class MobsLifecycle : MonoBehaviour, IDamagable
{
    private MobsUIController _mobsUIController;
    private EnemyAI _enemyAI;

    [SerializeField] private int health;

    private void Awake()
    {
        _mobsUIController = GetComponent<MobsUIController>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        WeaponSOViewer weapon = other.GetComponent<WeaponSOViewer>();
        if (weapon)
        {
            Damage(weapon.damage);
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
}
