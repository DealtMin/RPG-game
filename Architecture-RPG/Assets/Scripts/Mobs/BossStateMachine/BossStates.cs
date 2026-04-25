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

    public AggresiveState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        _stateMachine.Boss.SetChaising(true);
        _stateMachine.Boss.Animator.SetTrigger("Walk");
    }
    public override void Exit()
    {
        _stateMachine.Boss.SetChaising(false);
    }

    public override void LogicUpdate()
    {
        _stateMachine.Boss.ChasePlayer();
        if (_stateMachine.Boss.IsPlayerNear())
            _stateMachine.ChangeState(_stateMachine.CreateAttackState());
        else if (!_stateMachine.Boss.IsPlayerInView())
        {
            _stateMachine.ChangeState(new StayState(_stateMachine));
        }
    }
}
public class AttackState : AbstractBossState
{
    public AttackState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Boss.Animator.SetTrigger("RangeAttack");
    public override void LogicUpdate()
    {
        if (!_stateMachine.Boss.IsAttackReady())
            _stateMachine.ChangeState(new WaitToAttackState(_stateMachine));
        else 
        {
            _stateMachine.Boss.RangeAttack();
            _stateMachine.Boss.SetAttackCoolDown();
        }
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
            _stateMachine.ChangeState(_stateMachine.CreateAttackState());
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

public class Phase2AttackState : AbstractBossState
{
    public Phase2AttackState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Boss.Animator.SetTrigger("Attack");
    public override void LogicUpdate()
    {
        if (!_stateMachine.Boss.IsAttackReady())
            _stateMachine.ChangeState(new WaitToAttackState(_stateMachine));
        else _stateMachine.Boss.SetAttackCoolDown();
    }
}
