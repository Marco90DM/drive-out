namespace DriveOut.Core.Hazards;

public interface IHazardTarget
{
    void ApplyDrift(float driftFactor, float duration);
    void ApplySlow(float speedMultiplier, float duration);
    void ApplyDamage(float amount);
    void ApplyVisibilityImpair(float duration);
}
