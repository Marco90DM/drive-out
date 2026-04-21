using DriveOut.Core.Hazards;

namespace DriveOut.Gameplay.Hazards;

public sealed class CardboardHazard : IHazard
{
    private const float DefaultDamage = 5f;

    public HazardType Type => HazardType.Cardboard;
    public HazardEffect Effect { get; }

    public CardboardHazard(float damage = DefaultDamage)
    {
        if (damage <= 0f)
            throw new ArgumentOutOfRangeException(nameof(damage));

        Effect = new HazardEffect(HazardEffectType.Damage, damage, 0f);
    }

    public void OnHit(IHazardTarget target) =>
        target.ApplyDamage(Effect.Magnitude);
}
