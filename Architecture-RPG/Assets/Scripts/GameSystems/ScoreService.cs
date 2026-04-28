using System;

public class ScoreService : IScoreService
{
    public int CurrentScore { get; private set; } = 0;

    public event Action<int> OnScoreChanged;

    public void AddScore(int points)
    {
        if (points <= 0) return;

        CurrentScore += points;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void SetScore(int value)
    {
        CurrentScore = value;
        OnScoreChanged?.Invoke(CurrentScore);
    }
}