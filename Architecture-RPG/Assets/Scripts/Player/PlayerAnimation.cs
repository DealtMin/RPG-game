using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void Sprint(bool isSprinting)
    {
        animator.SetBool("IsRunning", isSprinting);
    }
    public void Walk(Vector2 move)
    {
        bool isWalk = move != Vector2.zero;
        animator.SetBool("Walking", isWalk);
    }
    public void PhysicAttack()
    {
        animator.Play("p_attack");
    }
    public void MagicAttack()
    {
        animator.Play("m_attack");
    }
}
