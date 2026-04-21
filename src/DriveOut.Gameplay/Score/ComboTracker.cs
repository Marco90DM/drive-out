using DriveOut.Core.Score;

namespace DriveOut.Gameplay.Score;

public sealed class ComboTracker : IComboTracker
{
    // Ordered highest-threshold first for early-exit lookup
    private static readonly (int MinStreak, float Multiplier)[] Thresholds =
    [
        (15, 5f),
        (10, 3f),
        (6,  2f),
        (3,  1.5f),
        (0,  1f),
    ];

    public int CurrentStreak { get; private set; }
    public float Multiplier => GetMultiplier(CurrentStreak);

    public event Action<int, float>? OnStreakChanged;  // (newStreak, newMultiplier)
    public event Action<int>? OnStreakBroken;           // (streakBeforeHit)

    public void RecordDodge()
    {
        CurrentStreak++;
        OnStreakChanged?.Invoke(CurrentStreak, Multiplier);
    }

    public void RecordHit()
    {
        if (CurrentStreak == 0) return;
        var broken = CurrentStreak;
        CurrentStreak = 0;
        OnStreakBroken?.Invoke(broken);
        OnStreakChanged?.Invoke(0, 1f);
    }

    public void Reset()
    {
        CurrentStreak = 0;
    }

    private static float GetMultiplier(int streak)
    {
        foreach (var (min, mult) in Thresholds)
            if (streak >= min) return mult;
        return 1f;
    }
}
