using System;

public class EnemyHealthController : IHealthController
{
    public event Action<int, int> TakeDamage;
    public event Action DeathEvent;
    private int _health;
    private int _maxHealth;

    public EnemyHealthController(int health)
    {
        _health = health;
        _maxHealth = health;
    }
    public void Damage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Death();
        }
        TakeDamage.Invoke(_health, _maxHealth);
    }

    public void Death()
    {
        DeathEvent.Invoke();
    }

    public int GetHealth() => _health;

    public void RestoreHealth(int value) //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        _health = value;
        TakeDamage.Invoke(value, _maxHealth);
    }
}
