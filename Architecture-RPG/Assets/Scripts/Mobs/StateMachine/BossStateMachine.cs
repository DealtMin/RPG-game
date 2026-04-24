public class BossStateMachine
{
    public AbstractBossState CurrentState { get; protected set; }
    public float SpeedFactor { get; protected set; }
    public EnemyBoss Boss { get; private set; }
    public BossStateMachine(EnemyBoss boss)
    {
        SpeedFactor = 1f;
        Boss = boss;
        CurrentState = new StayState(this);
    }
    public virtual void Initialize() =>
    CurrentState.Enter();
    public virtual AbstractBossState CreateAttackState() =>
    new AttackState(this);
    
    public virtual void ChangeState(AbstractBossState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }
}
