using UnityEngine;
using System.Collections;
using System;

public class MobsLifecycle : MonoBehaviour, IDamagable
{
    public event Action<int> EnemyTakeDamage;
    private EnemyAI _enemyAI;
    private bool _canDamage;
    [SerializeField] private float damageInvincibility = 1.5f;
    [SerializeField] private int health;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip hitClip;
    private IAudioService _audio;

    private void Start()
    {
        _canDamage = true;
        _enemyAI = GetComponent<EnemyAI>();
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
            StartCoroutine(DamageCountDown(damageInvincibility));
            EnemyTakeDamage.Invoke(health);
            damageParticles.Play();
            _audio.PlaySound(hitClip);
        }
    }

    public void Death()
    {
        _enemyAI.Death();
        damageParticles.Play();
    }
    
    public IEnumerator DamageCountDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _canDamage = true;        
    }
}
