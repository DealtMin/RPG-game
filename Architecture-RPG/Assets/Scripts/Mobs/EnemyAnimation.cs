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

    public void Attack()
    {
        _animator.Play("attack");
        _animator.SetBool("IsChaising", false);
    }
}
