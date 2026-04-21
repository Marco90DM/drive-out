using DriveOut.Core.Hazards;

namespace DriveOut.Gameplay.Hazards;

public sealed class EngineOilHazard : IHazard
{
    private const float DefaultSlowMultiplier = 0.5f;
    private const float DefaultDuration = 3f;

    public HazardType Type => HazardType.EngineOil;
    public HazardEffect Effect { get; }

    public EngineOilHazard(float slowMultiplier = DefaultSlowMultiplier, float duration = DefaultDuration)
    {
        if (slowMultiplier is <= 0f or >= 1f)
            throw new ArgumentOutOfRangeException(nameof(slowMultiplier), "Must be in range (0, 1).");
        if (duration <= 0f)
            throw new ArgumentOutOfRangeException(nameof(duration));

        Effect = new HazardEffect(HazardEffectType.Slow, slowMultiplier, duration);
    }

    public void OnHit(IHazardTarget target) =>
        target.ApplySlow(Effect.Magnitude, Effect.Duration);
}
