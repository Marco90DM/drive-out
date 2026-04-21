using DriveOut.Core.Vehicle;

namespace DriveOut.Core.Roles;

public sealed class RoleInputBuffer
{
    private readonly VehicleInput[] _driverInputs;

    public int Capacity { get; }

    public RoleInputBuffer(int capacity = 4)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        Capacity = capacity;
        _driverInputs = new VehicleInput[capacity];
    }

    public void SetDriverInput(int seatIndex, VehicleInput input)
    {
        ValidateIndex(seatIndex);
        _driverInputs[seatIndex] = input;
    }

    public VehicleInput GetDriverInput(int seatIndex)
    {
        ValidateIndex(seatIndex);
        return _driverInputs[seatIndex];
    }

    public void Clear(int seatIndex)
    {
        ValidateIndex(seatIndex);
        _driverInputs[seatIndex] = default;
    }

    public void ClearAll()
    {
        for (int i = 0; i < Capacity; i++)
            _driverInputs[i] = default;
    }

    private void ValidateIndex(int index)
    {
        if ((uint)index >= (uint)Capacity)
            throw new ArgumentOutOfRangeException(nameof(index));
    }
}
