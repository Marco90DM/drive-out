using DriveOut.Core.Boss;
using DriveOut.Core.Difficulty;
using DriveOut.Core.Hazards;
using DriveOut.Core.Metrics;
using DriveOut.Core.Upgrades;

namespace DriveOut.Core.Events;

public sealed record RunStartedEvent(DifficultyLevel Difficulty);
public sealed record RunEndedEvent(MetricsReport Report);
public sealed record PlayerDiedEvent(string Cause, float DistanceTraveled);
public sealed record HazardHitEvent(HazardType Type, float DamageDealt);
public sealed record UpgradePickedEvent(IUpgrade Upgrade);
public sealed record BossDefeatedEvent(BossPhase FinalPhase);
public sealed record ScoreChangedEvent(float NewScore, float Delta);
public sealed record HealthChangedEvent(float Current, float Max, float Delta);
