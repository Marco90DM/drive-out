using DriveOut.Core.Hazards;

namespace DriveOut.Gameplay.Hazards;

public sealed class BananaPeelHazard : IHazard
{
    private const float DefaultDriftFactor = 0.85f;
    private const float DefaultDuration = 2.5f;

    public HazardType Type => HazardType.BananaPeel;
    public HazardEffect Effect { get; }

    public BananaPeelHazard(float driftFactor = DefaultDriftFactor, float duration = DefaultDuration)
    {
        if (driftFactor is <= 0f or > 1f)
            throw new ArgumentOutOfRangeException(nameof(driftFactor), "Must be in range (0, 1].");
        if (duration <= 0f)
            throw new ArgumentOutOfRangeException(nameof(duration), "Must be greater than zero.");

        Effect = new HazardEffect(HazardEffectType.Drift, driftFactor, duration);
    }

    public void OnHit(IHazardTarget target)
    {
        target.ApplyDrift(Effect.Magnitude, Effect.Duration);
    }
}
