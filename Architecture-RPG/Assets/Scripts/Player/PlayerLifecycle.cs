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
            health = Math.Clamp(health-damage, 0, 100);
            Debug.Log(health);
            if (health <= 0)
            {
                Death();
            }
            _canDamage = false;
            StartCoroutine(DamageCountDown(damageInvincibility));
            _playerUIController.ReduceHealth(health);
            damageParticles.Play();
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
