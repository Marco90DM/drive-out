using DriveOut.Core;

namespace DriveOut.Gameplay;

public sealed class PlayerVehicleState
{
    public float Speed { get; private set; }
    public bool SteeringLocked { get; private set; }
    public float SteeringLockRemainingSeconds { get; private set; }

    public void Tick(float deltaTimeSeconds)
    {
        if (!SteeringLocked)
        {
            return;
        }

        SteeringLockRemainingSeconds = MathF.Max(0, SteeringLockRemainingSeconds - deltaTimeSeconds);
        SteeringLocked = SteeringLockRemainingSeconds > 0;
    }

    public void SetSpeed(float speed)
    {
        Speed = MathF.Max(0, speed);
    }

    public void LockSteering(float durationSeconds)
    {
        SteeringLocked = durationSeconds > 0;
        SteeringLockRemainingSeconds = MathF.Max(0, durationSeconds);
    }
}

public sealed class HazardResolver(GameConfig config)
{
    public void Apply(HazardType hazard, PlayerVehicleState vehicle)
    {
        switch (hazard)
        {
            case HazardType.BananaPeel:
                vehicle.LockSteering(config.BananaSteeringLockSeconds);
                break;
            case HazardType.EngineOil:
                vehicle.SetSpeed(vehicle.Speed * 0.8f);
                break;
            case HazardType.Spikes:
                vehicle.SetSpeed(vehicle.Speed * 0.6f);
                break;
            case HazardType.Cardboard:
            case HazardType.LightFlashing:
            default:
                break;
        }
    }
}
