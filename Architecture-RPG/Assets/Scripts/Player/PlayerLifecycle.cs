using System;
using UnityEngine;
using System.Collections;

public class PlayerLifecycle : MonoBehaviour, IDamagable
{
    public event Action<int> PlayerTakeDamage;
    public event Action PlayerDeath;
    private PlayerInputHandler _inputHandler;
    private bool _canDamage;
    private IAudioService _audio;
    [SerializeField] private int health;
    [SerializeField] private float damageInvincibility = 2f;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip hitClip;
    
    
    void Start()
    {
        _canDamage = true;
        _inputHandler = GetComponent<PlayerInputHandler>();
        _audio = ServiceLocator.Get<IAudioService>();
        }

    public void Damage(int damage)
    {
        if (_canDamage)
        {
            PlayerTakeDamage.Invoke(health);
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
        PlayerDeath.Invoke();
        _inputHandler.DisableInput();
    }

    public int GetHealth() => health;

// Позволяет загрузить ХП и обновить UI
    public void RestoreHealth(int value) {
        health = value;
        PlayerTakeDamage.Invoke(value);
    }
}
