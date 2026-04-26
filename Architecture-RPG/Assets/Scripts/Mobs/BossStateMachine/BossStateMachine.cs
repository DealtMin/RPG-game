public class BossStateMachine
{
    public AbstractBossState CurrentState { get; protected set; }
    public EnemyBoss Boss { get; private set; }
    public BossStateMachine(EnemyBoss boss)
    {
        Boss = boss;
        CurrentState = new BossStayState(this);
    }

    public virtual void ChangeState(AbstractBossState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }
    public virtual void Initialize() =>
    CurrentState.Enter();
    public virtual AbstractBossState CreateAttackState() =>
    new BossAttackState(this);
}

public class Phase2BossFightStateMachine : BossStateMachine
{
    public Phase2BossFightStateMachine(EnemyBoss boss) : base(boss) { }
    public override AbstractBossState CreateAttackState() =>
    new Phase2AttackState(this);
}

public class BossPassiveStateMachine : BossStateMachine
{
    public BossPassiveStateMachine(EnemyBoss boss) : base(boss)
    {
        CurrentState = new BossPassiveState(this);
    }
}
