public abstract class AbstractBossState
{
    protected BossStateMachine _stateMachine;
    public AbstractBossState(BossStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public abstract void AnimationUpdate();
    public abstract void PhysicsUpdate();
    public abstract void LogicUpdate();
}
