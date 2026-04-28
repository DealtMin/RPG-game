using System;

public interface IScoreService
{
    int CurrentScore { get; }
    
    void AddScore(int points);
    void ResetScore();

    event Action<int> OnScoreChanged;
}