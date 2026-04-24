using System.Diagnostics;

public class StayState : AbstractBossState
{
    public StayState(BossStateMachine stateMachine) : base(stateMachine) { }
    public override void AnimationUpdate() =>
    _stateMachine.Boss.Animator.Play("Stay");
    public override void PhysicsUpdate(){}
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
        _stateMachine.Boss.Say("Я вижу тебя! Ха-Ха-Ха-Ха");
        _playerLost = false;
    }
    public override void Exit()
    {
        if (_playerLost)
            _stateMachine.Boss.Say("Черт! Куда он делся?");
    }
    public override void AnimationUpdate() =>
    _stateMachine.Boss.Animator.Play("Ходьба");
    public override void PhysicsUpdate(){}
    public override void LogicUpdate()
    {
        if (_stateMachine.Boss.IsPlayerNear() && _stateMachine.Boss.IsAttackReady())
            _stateMachine.ChangeState(_stateMachine.CreateAttackState());
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
    _stateMachine.Boss.Say("Тебе конец!");
    public override void AnimationUpdate() =>
    _stateMachine.Boss.Animator.Play("Удар");
    public override void PhysicsUpdate(){}
    public override void LogicUpdate() =>
    _stateMachine.ChangeState(new AggresiveState(_stateMachine));
}