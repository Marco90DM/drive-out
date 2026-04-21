using DriveOut.Core.Hazards;

namespace DriveOut.Gameplay.Hazards;

public sealed class SpikesHazard : IHazard
{
    private const float DefaultDamage = 25f;

    public HazardType Type => HazardType.Spikes;
    public HazardEffect Effect { get; }

    public SpikesHazard(float damage = DefaultDamage)
    {
        if (damage <= 0f)
            throw new ArgumentOutOfRangeException(nameof(damage));

        Effect = new HazardEffect(HazardEffectType.Damage, damage, 0f);
    }

    public void OnHit(IHazardTarget target) =>
        target.ApplyDamage(Effect.Magnitude);
}
