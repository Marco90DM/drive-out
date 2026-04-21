namespace DriveOut.Core.Vehicle;

public readonly record struct VehicleInput(
    float Throttle,   // 0..1
    float Brake,      // 0..1
    float Steer       // -1 left .. +1 right
);
