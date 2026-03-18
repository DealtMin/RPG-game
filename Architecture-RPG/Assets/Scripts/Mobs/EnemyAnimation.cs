using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;
    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
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
}
