using DriveOut.Core.Hazards;

namespace DriveOut.Gameplay.Hazards;

public sealed class LightFlashingHazard : IHazard
{
    private const float DefaultDuration = 4f;

    public HazardType Type => HazardType.LightFlashing;
    public HazardEffect Effect { get; }

    public LightFlashingHazard(float duration = DefaultDuration)
    {
        if (duration <= 0f)
            throw new ArgumentOutOfRangeException(nameof(duration));

        Effect = new HazardEffect(HazardEffectType.VisibilityImpair, 1f, duration);
    }

    public void OnHit(IHazardTarget target) =>
        target.ApplyVisibilityImpair(Effect.Duration);
}
