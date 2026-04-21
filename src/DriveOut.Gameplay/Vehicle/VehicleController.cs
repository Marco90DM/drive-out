using DriveOut.Core.Hazards;
using DriveOut.Core.Vehicle;

namespace DriveOut.Gameplay.Vehicle;

public sealed class VehicleController : IVehicleController, IHazardTarget
{
    private readonly float _acceleration;
    private readonly float _deceleration;
    private readonly float _maxSteerAngle;
    private readonly float _steerSpeed;

    private float _slowMultiplier = 1f;
    private float _slowTimeRemaining;

    public VehicleState State { get; } = new();
    public float MaxSpeed { get; }

    public VehicleController(
        float maxSpeed = 30f,
        float acceleration = 15f,
        float deceleration = 20f,
        float maxSteerAngle = 35f,
        float steerSpeed = 120f)
    {
        if (maxSpeed <= 0f) throw new ArgumentOutOfRangeException(nameof(maxSpeed));
        if (acceleration <= 0f) throw new ArgumentOutOfRangeException(nameof(acceleration));

        MaxSpeed = maxSpeed;
        _acceleration = acceleration;
        _deceleration = deceleration;
        _maxSteerAngle = maxSteerAngle;
        _steerSpeed = steerSpeed;
    }

    public void Update(float deltaTime, VehicleInput input)
    {
        if (deltaTime <= 0f) return;

        TickTimers(deltaTime);

        var effectiveMax = MaxSpeed * _slowMultiplier;

        if (input.Throttle > 0f)
            State.Speed = Math.Min(effectiveMax, State.Speed + _acceleration * input.Throttle * deltaTime);
        else if (input.Brake > 0f)
            State.Speed = Math.Max(0f, State.Speed - _deceleration * input.Brake * deltaTime);

        var targetSteer = input.Steer * _maxSteerAngle;
        State.SteerAngle = MoveTowards(State.SteerAngle, targetSteer, _steerSpeed * deltaTime);

        if (State.IsDrifting)
        {
            State.LateralVelocity = input.Steer * State.Speed * State.DriftFactor;
            State.DriftTimeRemaining = Math.Max(0f, State.DriftTimeRemaining - deltaTime);
        }
        else
        {
            State.LateralVelocity = MoveTowards(State.LateralVelocity, 0f, State.Speed * deltaTime);
        }
    }

    public void ApplyDrift(float driftFactor, float duration)
    {
        if (driftFactor <= 0f || duration <= 0f) return;
        State.DriftFactor = Math.Clamp(driftFactor, 0f, 1f);
        State.DriftTimeRemaining = duration;
    }

    public void ApplySlow(float speedMultiplier, float duration)
    {
        if (speedMultiplier is <= 0f or >= 1f || duration <= 0f) return;
        _slowMultiplier = speedMultiplier;
        _slowTimeRemaining = duration;
    }

    public void ApplyDamage(float amount) { }

    public void ApplyVisibilityImpair(float duration) { }

    public void Reset()
    {
        State.Speed = 0f;
        State.LateralVelocity = 0f;
        State.SteerAngle = 0f;
        State.DriftFactor = 0f;
        State.DriftTimeRemaining = 0f;
        _slowMultiplier = 1f;
        _slowTimeRemaining = 0f;
    }

    private void TickTimers(float deltaTime)
    {
        if (_slowTimeRemaining > 0f)
        {
            _slowTimeRemaining = Math.Max(0f, _slowTimeRemaining - deltaTime);
            if (_slowTimeRemaining == 0f) _slowMultiplier = 1f;
        }
    }

    private static float MoveTowards(float current, float target, float maxDelta)
    {
        var diff = target - current;
        if (Math.Abs(diff) <= maxDelta) return target;
        return current + Math.Sign(diff) * maxDelta;
    }
}
