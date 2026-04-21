using DriveOut.Core.Roles;

namespace DriveOut.Gameplay.Roles;

public sealed class SeatManager : ISeatManager
{
    public const int MaxPlayers = 4;

    private readonly PlayerSeat[] _seats;

    int ISeatManager.MaxPlayers => MaxPlayers;
    public int OccupiedCount => _seats.Count(s => s.IsOccupied);
    public IReadOnlyList<PlayerSeat> Seats => _seats;

    public event Action<PlayerSeat>? OnPlayerJoined;
    public event Action<PlayerSeat>? OnPlayerLeft;
    public event Action<PlayerSeat, PlayerRole>? OnRoleSwitched;

    public SeatManager()
    {
        _seats = new PlayerSeat[MaxPlayers];
        for (int i = 0; i < MaxPlayers; i++)
            _seats[i] = new PlayerSeat(i);
    }

    public bool TryJoin(int seatIndex, PlayerRole role)
    {
        if ((uint)seatIndex >= MaxPlayers) return false;
        var seat = _seats[seatIndex];
        if (seat.IsOccupied) return false;
        if (!IsRoleAvailable(role)) return false;

        seat.Join(role);
        OnPlayerJoined?.Invoke(seat);
        return true;
    }

    public bool TryLeave(int seatIndex)
    {
        if ((uint)seatIndex >= MaxPlayers) return false;
        var seat = _seats[seatIndex];
        if (!seat.IsOccupied) return false;

        seat.Leave();
        OnPlayerLeft?.Invoke(seat);
        return true;
    }

    public bool TrySwitchRole(int seatIndex, PlayerRole newRole)
    {
        if ((uint)seatIndex >= MaxPlayers) return false;
        var seat = _seats[seatIndex];
        if (!seat.IsOccupied) return false;
        if (seat.Role == newRole) return false;

        // Driver role is exclusive: can't switch to Driver if another seat already holds it
        if (newRole == PlayerRole.Driver && !IsRoleAvailable(newRole)) return false;

        var previous = seat.Role;
        seat.ChangeRole(newRole);
        OnRoleSwitched?.Invoke(seat, previous);
        return true;
    }

    // Driver role can only be held by one player at a time
    private bool IsRoleAvailable(PlayerRole role) =>
        role != PlayerRole.Driver || !_seats.Any(s => s.IsOccupied && s.Role == PlayerRole.Driver);
}
