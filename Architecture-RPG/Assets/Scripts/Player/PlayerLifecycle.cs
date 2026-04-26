using System;
using UnityEngine;
using System.Collections;

public class PlayerLifecycle : MonoBehaviour, IDamagable
{
    public event Action<int, int> TakeDamage;
    public event Action DeathEvent;
    private PlayerInputHandler _inputHandler;
    private bool _canDamage;
    private IAudioService _audio;
    [SerializeField] private int health;
    [SerializeField] private float damageInvincibility = 2f;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip hitClip;
    public int maxHealth { private set; get; }
    
    
    void Start()
    {
        _canDamage = true;
        _inputHandler = GetComponent<PlayerInputHandler>();
        _audio = ServiceLocator.Get<IAudioService>();
        maxHealth = health;
    }

    public void Damage(int damage)
    {
        if (_canDamage)
        {
            TakeDamage.Invoke(health, maxHealth);
            health = Math.Clamp(health - damage, 0, 100);
            _canDamage = false;
            damageParticles.Play();
            
            if (health <= 0)
            {
                Death();
            }
            else
            {
                StartCoroutine(DamageCountDown(damageInvincibility));
                _audio.PlaySound(hitClip);
            }
        }
    }
       

    
    private IEnumerator DamageCountDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _canDamage = true;        
    }

    public void Death()
    {
        DeathEvent.Invoke();
        _inputHandler.DisableInput();
    }

    public int GetHealth() => health;

// Позволяет загрузить ХП и обновить UI
    public void RestoreHealth(int value) {
        health = value;
        TakeDamage.Invoke(value, maxHealth);
    }
}
