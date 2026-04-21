namespace DriveOut.Core.Vehicle;

public sealed class VehicleState
{
    public float Speed { get; set; }
    public float LateralVelocity { get; set; }
    public float SteerAngle { get; set; }
    public float DriftFactor { get; set; }
    public float DriftTimeRemaining { get; set; }

    public bool IsDrifting => DriftTimeRemaining > 0f;
}
