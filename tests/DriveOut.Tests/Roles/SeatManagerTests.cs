using DriveOut.Core.Roles;
using DriveOut.Gameplay.Roles;

namespace DriveOut.Tests.Roles;

public sealed class SeatManagerTests
{
    private static SeatManager Make() => new();

    // --- TryJoin ---

    [Fact]
    public void TryJoin_ValidSeatAndRole_ReturnsTrue()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Driver).Should().BeTrue();
    }

    [Fact]
    public void TryJoin_SeatOccupied_ReturnsFalse()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Driver);
        mgr.TryJoin(0, PlayerRole.Shooter).Should().BeFalse();
    }

    [Fact]
    public void TryJoin_InvalidIndex_ReturnsFalse()
    {
        var mgr = Make();
        mgr.TryJoin(-1, PlayerRole.Driver).Should().BeFalse();
        mgr.TryJoin(4, PlayerRole.Driver).Should().BeFalse();
    }

    [Fact]
    public void TryJoin_DriverAlreadyTaken_ReturnsFalse()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Driver);
        mgr.TryJoin(1, PlayerRole.Driver).Should().BeFalse();
    }

    [Fact]
    public void TryJoin_NonDriverRoles_CanBeMultiple()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Shooter).Should().BeTrue();
        mgr.TryJoin(1, PlayerRole.Shooter).Should().BeTrue();
    }

    [Fact]
    public void TryJoin_SetsOccupiedAndRole()
    {
        var mgr = Make();
        mgr.TryJoin(2, PlayerRole.Navigator);

        mgr.Seats[2].IsOccupied.Should().BeTrue();
        mgr.Seats[2].Role.Should().Be(PlayerRole.Navigator);
    }

    [Fact]
    public void TryJoin_FiresOnPlayerJoined()
    {
        var mgr = Make();
        PlayerSeat? fired = null;
        mgr.OnPlayerJoined += s => fired = s;

        mgr.TryJoin(0, PlayerRole.Driver);

        fired.Should().NotBeNull();
        fired!.Index.Should().Be(0);
    }

    [Fact]
    public void OccupiedCount_TracksJoins()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Driver);
        mgr.TryJoin(1, PlayerRole.Shooter);
        mgr.OccupiedCount.Should().Be(2);
    }

    // --- TryLeave ---

    [Fact]
    public void TryLeave_OccupiedSeat_ReturnsTrue()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Driver);
        mgr.TryLeave(0).Should().BeTrue();
    }

    [Fact]
    public void TryLeave_EmptySeat_ReturnsFalse()
    {
        var mgr = Make();
        mgr.TryLeave(0).Should().BeFalse();
    }

    [Fact]
    public void TryLeave_ClearsOccupied()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Driver);
        mgr.TryLeave(0);
        mgr.Seats[0].IsOccupied.Should().BeFalse();
    }

    [Fact]
    public void TryLeave_FiresOnPlayerLeft()
    {
        var mgr = Make();
        mgr.TryJoin(1, PlayerRole.Shooter);
        PlayerSeat? fired = null;
        mgr.OnPlayerLeft += s => fired = s;

        mgr.TryLeave(1);

        fired!.Index.Should().Be(1);
    }

    [Fact]
    public void TryLeave_DriverLeaves_NewDriverCanJoin()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Driver);
        mgr.TryLeave(0);
        mgr.TryJoin(1, PlayerRole.Driver).Should().BeTrue();
    }

    // --- TrySwitchRole ---

    [Fact]
    public void TrySwitchRole_ValidSwitch_ReturnsTrue()
    {
        var mgr = Make();
        mgr.TryJoin(1, PlayerRole.Shooter);
        mgr.TrySwitchRole(1, PlayerRole.Navigator).Should().BeTrue();
    }

    [Fact]
    public void TrySwitchRole_SameRole_ReturnsFalse()
    {
        var mgr = Make();
        mgr.TryJoin(1, PlayerRole.Shooter);
        mgr.TrySwitchRole(1, PlayerRole.Shooter).Should().BeFalse();
    }

    [Fact]
    public void TrySwitchRole_EmptySeat_ReturnsFalse()
    {
        var mgr = Make();
        mgr.TrySwitchRole(0, PlayerRole.Driver).Should().BeFalse();
    }

    [Fact]
    public void TrySwitchRole_DriverOccupied_CannotSwitchToDriver()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Driver);
        mgr.TryJoin(1, PlayerRole.Shooter);
        mgr.TrySwitchRole(1, PlayerRole.Driver).Should().BeFalse();
    }

    [Fact]
    public void TrySwitchRole_UpdatesRole()
    {
        var mgr = Make();
        mgr.TryJoin(2, PlayerRole.Shooter);
        mgr.TrySwitchRole(2, PlayerRole.Engineer);
        mgr.Seats[2].Role.Should().Be(PlayerRole.Engineer);
    }

    [Fact]
    public void TrySwitchRole_FiresOnRoleSwitchedWithPreviousRole()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Shooter);
        PlayerRole? previous = null;
        mgr.OnRoleSwitched += (_, prev) => previous = prev;

        mgr.TrySwitchRole(0, PlayerRole.Navigator);

        previous.Should().Be(PlayerRole.Shooter);
    }

    [Fact]
    public void TrySwitchRole_ToDriver_WhenDriverSeatFree_Succeeds()
    {
        var mgr = Make();
        mgr.TryJoin(0, PlayerRole.Shooter);
        mgr.TrySwitchRole(0, PlayerRole.Driver).Should().BeTrue();
        mgr.Seats[0].Role.Should().Be(PlayerRole.Driver);
    }

    // --- Seats ---

    [Fact]
    public void Seats_Count_IsMaxPlayers()
    {
        var mgr = Make();
        mgr.Seats.Should().HaveCount(SeatManager.MaxPlayers);
    }

    [Fact]
    public void Seats_InitiallyAllEmpty()
    {
        var mgr = Make();
        mgr.Seats.Should().AllSatisfy(s => s.IsOccupied.Should().BeFalse());
    }
}
