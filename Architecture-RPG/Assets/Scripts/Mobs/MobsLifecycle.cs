using UnityEngine;
using System.Collections;
using System;

public class MobsLifecycle : MonoBehaviour, IHealthController, IDamagable
{
    public event Action<int> TakeDamage;
    public event Action DeathEvent;

    private bool _canDamage;
    [SerializeField] private float damageInvincibility = 1.5f;
    [SerializeField] private int health;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip hitClip;
    private IAudioService _audio;

    private void Awake()
    {
        _canDamage = true;
        _audio = ServiceLocator.Get<IAudioService>();
    }

    public void Damage(int damage)
    {
        if (_canDamage)
        {
            health -= damage;
            if (health <= 0)
            {
                Death();
            }
            _canDamage = false;
            TakeDamage.Invoke(health);
            damageParticles.Play();
            _audio.PlaySound(hitClip);
        }
    }

    public void Death()
    {
        DeathEvent.Invoke();
        damageParticles.Play();
    }
    
    public IEnumerator DamageCountDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _canDamage = true;        
    }

    public int GetHealth() => health;

    public void RestoreHealth(int value) {
        health = value;
        TakeDamage.Invoke(value);
    }
}
