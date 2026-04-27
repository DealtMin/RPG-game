using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator _animator;
    PlayerCombat _playerCombat;
    PlayerLifecycle _playerLifecycle;
    PlayerInputHandler _playerInputHandler;
    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerCombat = GetComponent<PlayerCombat>();
        _playerLifecycle = GetComponent<PlayerLifecycle>();
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        EventHandle();
    }
    private void EventHandle()
    {
        _playerInputHandler.OnMoveInput += Walk;
        _playerInputHandler.OnSprintPressed += SprintStart;
        _playerInputHandler.OnSprintReleased += SprintStop;
        _playerLifecycle.DeathEvent += Death;
        _playerCombat.PlayerAttackPhysical += PhysicAttack;
        _playerCombat.PlayerAttackMagic += MagicAttack;
    }
    public void SprintStart()
    {
        _animator.SetBool("IsRunning", true);
    }
    public void SprintStop()
    {
        _animator.SetBool("IsRunning", false);
    }   
    public void Walk(Vector2 move)
    {
        bool isWalk = move != Vector2.zero;
        _animator.SetBool("Walking", isWalk);
    }
    public void PhysicAttack()
    {
        _animator.Play("p_attack");
    }
    public void MagicAttack()
    {
        _animator.Play("m_attack");
    }
    public void Death()
    {
        _animator.Play("death");
    }
    void OnDestroy()
    {
        _playerInputHandler.OnMoveInput -= Walk;
        _playerInputHandler.OnSprintPressed -= SprintStart;
        _playerInputHandler.OnSprintReleased -= SprintStop;
        _playerLifecycle.DeathEvent -= Death;
        _playerCombat.PlayerAttackPhysical -= PhysicAttack;
        _playerCombat.PlayerAttackMagic -= MagicAttack;
    }
}
