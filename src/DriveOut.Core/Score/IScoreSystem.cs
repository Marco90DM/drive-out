namespace DriveOut.Core.Score;

public interface IScoreSystem
{
    int Score { get; }
    float Distance { get; }

    event Action<int> OnScoreChanged;
    event Action<float> OnDistanceChanged;

    void AddScore(int points);
    void AddDistance(float delta);
    void Reset();
}
