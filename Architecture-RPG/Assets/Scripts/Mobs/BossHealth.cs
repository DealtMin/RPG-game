using System;

public class BossHealthController : IHealthController
{
    public event Action<int, int> TakeDamage;
    public event Action DeathEvent;
    public event Action SecondPhase;
    private int _maxHealth;
    private int _health;

    public BossHealthController(int health)
    {
        _maxHealth = health;
        _health = health;
    }
    public void Damage(int damage)
    {
        _health -= damage;
        if (_health <= _maxHealth/2)
            SecondPhase.Invoke();
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
    public int GetMaxHealth() => _maxHealth;

    public void RestoreHealth(int value)
    {
        _health = value;
        if (_health <= _maxHealth/2) SecondPhase.Invoke();
    }
}
