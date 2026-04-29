using System;

public interface IScoreService
{
    int CurrentScore { get; }
    
    void AddScore(int points);
    void ResetScore();
    void SetScore(int value);
    event Action<int> OnScoreChanged;
}