public class EnemyStateMachine
{    
    public AbstractEnemyState CurrentState { get; protected set; }
    public Enemy Enemy { get; private set; }
    public EnemyStateMachine(Enemy enemy)
    {
        Enemy = enemy;
        CurrentState = new EnemyStayState(this);
    }

    public virtual void ChangeState(AbstractEnemyState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }
    public virtual void Initialize() =>
    CurrentState.Enter();
    public virtual AbstractEnemyState CreateAttackState() =>
    new EnemyAttackState(this);
}
public class EnemyPassiveStateMachine : EnemyStateMachine
{
    public EnemyPassiveStateMachine(Enemy enemy) : base(enemy)
    {
        CurrentState = new EnemyPassiveState(this);
    }
}
