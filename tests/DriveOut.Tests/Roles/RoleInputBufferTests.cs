using DriveOut.Core.Roles;
using DriveOut.Core.Vehicle;

namespace DriveOut.Tests.Roles;

public sealed class RoleInputBufferTests
{
    [Fact]
    public void Constructor_InvalidCapacity_Throws()
    {
        var act = () => new RoleInputBuffer(0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetDriverInput_DefaultIsZero()
    {
        var buf = new RoleInputBuffer(4);
        var input = buf.GetDriverInput(0);
        input.Throttle.Should().Be(0f);
        input.Brake.Should().Be(0f);
        input.Steer.Should().Be(0f);
    }

    [Fact]
    public void SetAndGet_DriverInput_RoundTrips()
    {
        var buf = new RoleInputBuffer(4);
        var written = new VehicleInput(Throttle: 1f, Brake: 0f, Steer: 0.5f);
        buf.SetDriverInput(0, written);
        buf.GetDriverInput(0).Should().Be(written);
    }

    [Fact]
    public void SetDriverInput_DifferentSeats_AreIndependent()
    {
        var buf = new RoleInputBuffer(4);
        buf.SetDriverInput(0, new VehicleInput(Throttle: 1f, Brake: 0f, Steer: 0f));
        buf.SetDriverInput(1, new VehicleInput(Throttle: 0f, Brake: 1f, Steer: -1f));

        buf.GetDriverInput(0).Throttle.Should().Be(1f);
        buf.GetDriverInput(1).Brake.Should().Be(1f);
    }

    [Fact]
    public void SetDriverInput_OutOfRange_Throws()
    {
        var buf = new RoleInputBuffer(2);
        var act = () => buf.SetDriverInput(2, default);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetDriverInput_OutOfRange_Throws()
    {
        var buf = new RoleInputBuffer(2);
        var act = () => buf.GetDriverInput(-1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Clear_ResetsOneSeat()
    {
        var buf = new RoleInputBuffer(4);
        buf.SetDriverInput(1, new VehicleInput(Throttle: 1f, Brake: 0f, Steer: 0f));
        buf.Clear(1);
        buf.GetDriverInput(1).Throttle.Should().Be(0f);
    }

    [Fact]
    public void ClearAll_ResetsAllSeats()
    {
        var buf = new RoleInputBuffer(4);
        for (int i = 0; i < 4; i++)
            buf.SetDriverInput(i, new VehicleInput(Throttle: 1f, Brake: 0f, Steer: 0f));

        buf.ClearAll();

        for (int i = 0; i < 4; i++)
            buf.GetDriverInput(i).Throttle.Should().Be(0f);
    }
}
