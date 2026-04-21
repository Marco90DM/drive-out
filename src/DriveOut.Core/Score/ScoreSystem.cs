namespace DriveOut.Core.Score;

public sealed class ScoreSystem : IScoreSystem
{
    public int Score { get; private set; }
    public float Distance { get; private set; }

    public event Action<int>? OnScoreChanged;
    public event Action<float>? OnDistanceChanged;

    public void AddScore(int points)
    {
        if (points <= 0) return;

        Score += points;
        OnScoreChanged?.Invoke(Score);
    }

    public void AddDistance(float delta)
    {
        if (delta <= 0f) return;

        Distance += delta;
        OnDistanceChanged?.Invoke(Distance);
    }

    public void Reset()
    {
        Score = 0;
        Distance = 0f;
    }
}
