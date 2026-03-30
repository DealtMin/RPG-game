using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;
    private EnemyAI _enemyAI;
    private MobsLifecycle _mobsLifecycle;
    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _enemyAI = GetComponent<EnemyAI>();
        _mobsLifecycle = GetComponent<MobsLifecycle>();
        EventHandle();
    }
    private void EventHandle()
    {
        _enemyAI.EnemyAttack += Attack;
        _enemyAI.EnemyChaising += Chase;
        _enemyAI.EnemyIdle += Idle;
        _mobsLifecycle.EnemyDeath += DeathAnimation;
    }
    public void Idle()
    {
        _animator.SetBool("IsChaising", false);
    }

    public void DeathAnimation()
    {
        _animator.Play("death");
    }
    public void Chase()
    {
        _animator.SetBool("IsChaising", true);
    }

    public void Attack(bool isAttacking)
    {
        _animator.SetBool("IsChaising", false);
        if (isAttacking)
        {
            _animator.Play("attack", -1, 0f);
        }
    }
    void OnDestroy()
    {
        _enemyAI.EnemyAttack -= Attack;
        _enemyAI.EnemyChaising -= Chase;
        _enemyAI.EnemyIdle -= Idle;
        _mobsLifecycle.EnemyDeath -= DeathAnimation;
    }
}
