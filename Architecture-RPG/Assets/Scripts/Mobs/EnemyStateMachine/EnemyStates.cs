public class EnemyStayState : AbstractEnemyState
{
    public EnemyStayState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
   _stateMachine.Enemy.Animator.SetTrigger("Idle");
    public override void LogicUpdate()
    {
        if (_stateMachine.Enemy.IsPlayerInView())
            _stateMachine.ChangeState(new EnemyAggresiveState(_stateMachine));
    }
}
public class EnemyPassiveState : AbstractEnemyState
{
    public EnemyPassiveState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
   _stateMachine.Enemy.Animator.SetTrigger("Idle");
    public override void LogicUpdate()
    {
        if (_stateMachine.Enemy.IsPlayerInView())
            _stateMachine.ChangeState(new EnemyScareState(_stateMachine));
    }
}
public class EnemyScareState : AbstractEnemyState
{
    public EnemyScareState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        _stateMachine.Enemy.SetChaising(true);
        _stateMachine.Enemy.Animator.SetTrigger("Walk");
    }
    public override void Exit()
    {
        _stateMachine.Enemy.SetChaising(false);
    }

    public override void LogicUpdate()
    {
        _stateMachine.Enemy.Flee();
        if (!_stateMachine.Enemy.IsPlayerInView())
        {
            _stateMachine.ChangeState(new EnemyPassiveState(_stateMachine));
        }
    }
}
public class EnemyAggresiveState : AbstractEnemyState
{
    public EnemyAggresiveState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        _stateMachine.Enemy.SetChaising(true);
        _stateMachine.Enemy.Animator.SetTrigger("Walk");
    }
    public override void Exit()
    {
        _stateMachine.Enemy.SetChaising(false);
    }

    public override void LogicUpdate()
    {
        _stateMachine.Enemy.ChasePlayer();
        if (_stateMachine.Enemy.IsPlayerNear())
            _stateMachine.ChangeState(new EnemyWaitToAttackState(_stateMachine));
        else if (!_stateMachine.Enemy.IsPlayerInView())
        {
            _stateMachine.ChangeState(new EnemyStayState(_stateMachine));
        }
    }
}
public class EnemyAttackState : AbstractEnemyState
{
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Enemy.Animator.SetTrigger("Attack");
    public override void LogicUpdate()
    {
        _stateMachine.Enemy.Attack();
        _stateMachine.Enemy.SetAttackCoolDown();
        _stateMachine.ChangeState(new EnemyWaitToAttackState(_stateMachine));
    }
}

public class EnemyWaitToAttackState : AbstractEnemyState
{
    public EnemyWaitToAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Enemy.Animator.SetTrigger("Stop");
    public override void LogicUpdate()
    {
        if (!_stateMachine.Enemy.IsPlayerInView())
            _stateMachine.ChangeState(new EnemyStayState(_stateMachine));
        if (!_stateMachine.Enemy.IsPlayerNear() && _stateMachine.Enemy.IsPlayerInView())
            _stateMachine.ChangeState(new EnemyAggresiveState(_stateMachine));
        if (_stateMachine.Enemy.IsPlayerNear() && _stateMachine.Enemy.IsAttackReady())
            _stateMachine.ChangeState(new EnemyAttackState(_stateMachine));
    }
}

public class EnemyDeathState : AbstractEnemyState
{
    public EnemyDeathState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter() =>
    _stateMachine.Enemy.Animator.SetTrigger("Death");
    public override void LogicUpdate()
    {
    }
}
