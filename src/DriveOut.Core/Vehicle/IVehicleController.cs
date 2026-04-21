namespace DriveOut.Core.Vehicle;

public interface IVehicleController
{
    VehicleState State { get; }
    float MaxSpeed { get; }

    void Update(float deltaTime, VehicleInput input);
    void ApplyDrift(float driftFactor, float duration);
    void Reset();
}
