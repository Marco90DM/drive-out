namespace DriveOut.Core.Hazards;

public readonly record struct HazardEffect(
    HazardEffectType EffectType,
    float Magnitude,
    float Duration
);
