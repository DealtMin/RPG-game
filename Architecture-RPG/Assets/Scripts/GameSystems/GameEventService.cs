using System;

public class GameEventService : IGameEventService
{
    public event Action OnEnemyKilled;
    public event Action<int> OnKillCountChanged;

    public event Action OnBossShouldSpawn;
    public event Action OnVictoryConditionMet;

    public int CurrentKillCount { get; private set; } = 0;

    public void NotifyEnemyDeath()
    {
        CurrentKillCount++;

        OnEnemyKilled?.Invoke();
        OnKillCountChanged?.Invoke(CurrentKillCount);

        if (CurrentKillCount == 3)
        {
            OnBossShouldSpawn?.Invoke();
            
        }

        if (CurrentKillCount == 5)
        {
            OnVictoryConditionMet?.Invoke();
            
        }
    }

    public void ResetKillCount()
    {
        CurrentKillCount = 0;
    }
}