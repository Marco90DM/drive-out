namespace DriveOut.Core.Score;

public interface IComboTracker
{
    int CurrentStreak { get; }
    float Multiplier { get; }
    void RecordDodge();
    void RecordHit();
    void Reset();
}
