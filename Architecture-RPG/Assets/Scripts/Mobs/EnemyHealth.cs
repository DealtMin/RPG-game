using UnityEngine;
using System.Collections;
using System;

public class EnemyHealthController : IHealthController
{
    public event Action<int> TakeDamage;
    public event Action DeathEvent;
    public event Action SecondPhase;
    private int maxHealth;
    private int health;

    public EnemyHealthController(int health)
    {
        maxHealth = health;
        this.health = health;
    }
    public void Damage(int damage)
    {
        health -= damage;
        if (health <= maxHealth/2)
            SecondPhase.Invoke();
        if (health <= 0)
        {
            Death();
        }
        TakeDamage.Invoke(health);
    }

    public void Death()
    {
        DeathEvent.Invoke();
    }

    public int GetHealth() => health;

    public void RestoreHealth(int value)
    {
        health = value;
        TakeDamage.Invoke(value);
    }
}
