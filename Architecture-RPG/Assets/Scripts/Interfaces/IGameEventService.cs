using System;

public interface IGameEventService
{
    event Action OnEnemyKilled;
    event Action<int> OnKillCountChanged;

    event Action OnBossShouldSpawn;
    event Action OnVictoryConditionMet;

    void NotifyEnemyDeath();

    int CurrentKillCount { get; }

    void ResetKillCount();

    void SetKillCount(int value); 

   
}