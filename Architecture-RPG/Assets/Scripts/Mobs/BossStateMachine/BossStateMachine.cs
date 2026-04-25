public class BossStateMachine
{
    public AbstractBossState CurrentState { get; protected set; }
    public EnemyBoss Boss { get; private set; }
    public BossStateMachine(EnemyBoss boss)
    {
        Boss = boss;
        CurrentState = new StayState(this);
    }

    public virtual void ChangeState(AbstractBossState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }
}
