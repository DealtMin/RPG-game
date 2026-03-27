using System;
using UnityEngine;
using System.Collections;

public class PlayerLifecycle : MonoBehaviour, IDamagable
{
    private PlayerUIController _playerUIController;
    private PlayerAnimation _playerAnimation;
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
        _playerUIController = GetComponent<PlayerUIController>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _audio = ServiceLocator.Get<IAudioService>();
    }

    public void Damage(int damage)
    {
        if (_canDamage)
        {
            health = Math.Clamp(health - damage, 0, 100);
            _canDamage = false;
            _playerUIController.ReduceHealth(health);
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
        _playerAnimation.Death();
        _inputHandler.DisableInput();
    }
}
