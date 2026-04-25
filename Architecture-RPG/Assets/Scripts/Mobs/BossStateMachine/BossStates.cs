using System.Diagnostics;

public class StayState : AbstractBossState
{
    public StayState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
   _stateMachine.Boss.Animator.SetTrigger("Idle");
    public override void LogicUpdate()
    {
        if (_stateMachine.Boss.IsPlayerInView())
            _stateMachine.ChangeState(new AggresiveState(_stateMachine));
    }
}
public class AggresiveState : AbstractBossState
{
    private bool _playerLost;
    public AggresiveState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        _stateMachine.Boss.SetChaising(true);
        _stateMachine.Boss.Animator.SetTrigger("Walk");
        _stateMachine.Boss.Say("Я вижу тебя! Ха-Ха-Ха-Ха");
    }
    public override void Exit()
    {
        if (_playerLost)
            _stateMachine.Boss.Say("Черт! Куда он делся?");
        _stateMachine.Boss.SetChaising(false);
    }

    public override void LogicUpdate()
    {
        _stateMachine.Boss.ChasePlayer();
        if (_stateMachine.Boss.IsPlayerNear())
            _stateMachine.ChangeState(new AttackState(_stateMachine));
        else if (!_stateMachine.Boss.IsPlayerInView())
        {
            _playerLost = true;
            _stateMachine.ChangeState(new StayState(_stateMachine));
        }
    }
}
public class AttackState : AbstractBossState
{
    public AttackState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Boss.Animator.SetTrigger("Attack");
    public override void LogicUpdate()
    {
        if (!_stateMachine.Boss.IsAttackReady())
            _stateMachine.ChangeState(new WaitToAttackState(_stateMachine));
    }
}

public class WaitToAttackState : AbstractBossState
{
    public WaitToAttackState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Boss.Animator.SetTrigger("Stop");
    public override void LogicUpdate()
    {

        if (!_stateMachine.Boss.IsPlayerNear())
            _stateMachine.ChangeState(new AggresiveState(_stateMachine));
        if (_stateMachine.Boss.IsPlayerNear() && _stateMachine.Boss.IsAttackReady())
            _stateMachine.ChangeState(new AttackState(_stateMachine));
    }
}

public class DeathState : AbstractBossState
{
    public DeathState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Boss.Animator.SetTrigger("Death");
    public override void LogicUpdate()
    {
    }
}
