using System;
using UnityEngine; 

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

        CheckConditions();
    }

    public void ResetKillCount()
    {
        CurrentKillCount = 0;
        OnKillCountChanged?.Invoke(CurrentKillCount);
    }

    public void SetKillCount(int value)
    {
        CurrentKillCount = value;
        OnKillCountChanged?.Invoke(CurrentKillCount);
    }

    private void CheckConditions()
    {
        
        EnemySpawner spawner = ServiceLocator.Get<EnemySpawner>(); 
        bool bossAlreadySpawned = (spawner != null && spawner.HasBossSpawned);

        if (CurrentKillCount >= 3 && !bossAlreadySpawned)
        {
            OnBossShouldSpawn?.Invoke();
            Debug.Log($"[GameEventService] Boss spawn conditions met. Current kills: {CurrentKillCount}, Boss already spawned: {bossAlreadySpawned}");
        }

        if ( CurrentKillCount >= 5)
        {
            OnVictoryConditionMet?.Invoke();
            Debug.Log($"[GameEventService] Victory conditions met. Current kills: {CurrentKillCount}");
        }
    }
}