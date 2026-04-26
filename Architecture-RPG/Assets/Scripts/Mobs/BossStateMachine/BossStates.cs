public class BossStayState : AbstractBossState
{
    public BossStayState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
   _stateMachine.Boss.Animator.SetTrigger("Idle");
    public override void LogicUpdate()
    {
        if (_stateMachine.Boss.IsPlayerInView())
            _stateMachine.ChangeState(new BossAggresiveState(_stateMachine));
    }
}
public class BossPassiveState : AbstractBossState
{
    public BossPassiveState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
   _stateMachine.Boss.Animator.SetTrigger("Idle");
    public override void LogicUpdate()
    {
        if (_stateMachine.Boss.wasAttaked)
            _stateMachine.ChangeState(new BossAggresiveState(_stateMachine));
    }
}
public class BossAggresiveState : AbstractBossState
{

    public BossAggresiveState(BossStateMachine stateMachine) : base(stateMachine) { }
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
            _stateMachine.ChangeState(new BossWaitToAttackState(_stateMachine));
        else if (!_stateMachine.Boss.IsPlayerInView())
        {
            _stateMachine.ChangeState(new BossStayState(_stateMachine));
        }
    }
}
public class BossAttackState : AbstractBossState
{
    public BossAttackState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Boss.Animator.SetTrigger("RangeAttack");
    public override void LogicUpdate()
    {
        _stateMachine.Boss.RangeAttack();
        _stateMachine.Boss.SetAttackCoolDown();
        _stateMachine.ChangeState(new BossWaitToAttackState(_stateMachine));
    }
}

public class BossWaitToAttackState : AbstractBossState
{
    public BossWaitToAttackState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Boss.Animator.SetTrigger("Stop");
    public override void LogicUpdate()
    {
        if (!_stateMachine.Boss.IsPlayerInView())
            _stateMachine.ChangeState(new BossStayState(_stateMachine));
        if (!_stateMachine.Boss.IsPlayerNear() && _stateMachine.Boss.IsPlayerInView())
            _stateMachine.ChangeState(new BossAggresiveState(_stateMachine));
        if (_stateMachine.Boss.IsPlayerNear() && _stateMachine.Boss.IsAttackReady())
            _stateMachine.ChangeState(_stateMachine.CreateAttackState());
    }
}

public class BossDeathState : AbstractBossState
{
    public BossDeathState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Boss.Animator.SetTrigger("Death");
    public override void LogicUpdate(){}
}

public class Phase2AttackState : AbstractBossState
{
    public Phase2AttackState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Boss.Animator.SetTrigger("Attack");
    public override void LogicUpdate()
    {
        _stateMachine.Boss.SetAttackCoolDown();
        _stateMachine.ChangeState(new BossWaitToAttackState(_stateMachine));
    }
}
