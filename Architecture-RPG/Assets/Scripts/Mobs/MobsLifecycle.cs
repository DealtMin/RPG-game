using UnityEngine;
using System.Collections;

public class MobsLifecycle : MonoBehaviour, IDamagable
{
    private MobsUIController _mobsUIController;
    private EnemyAI _enemyAI;
    private bool _canDamage;
    [SerializeField] private float damageInvincibility = 1.5f;
    [SerializeField] private int health;

    private void Awake()
    {
        _canDamage = true;
        _mobsUIController = GetComponent<MobsUIController>();
        _enemyAI = GetComponent<EnemyAI>();
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
            _mobsUIController.ReduceHealth(health);
        }
    }

    public void Death()
    {
        _enemyAI.Death();
    }
    
    public IEnumerator DamageCountDown(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        _canDamage = true;        
    }
}
