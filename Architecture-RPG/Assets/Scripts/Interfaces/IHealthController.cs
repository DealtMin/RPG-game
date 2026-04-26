using UnityEngine;
using System;

public interface IHealthController
{
    event Action<int, int> TakeDamage;
    event Action DeathEvent;
    void Damage(int damage);
    int GetHealth();
    void RestoreHealth(int value);
}
