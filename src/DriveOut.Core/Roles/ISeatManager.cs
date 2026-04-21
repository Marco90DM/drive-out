namespace DriveOut.Core.Roles;

public interface ISeatManager
{
    int MaxPlayers { get; }
    int OccupiedCount { get; }
    IReadOnlyList<PlayerSeat> Seats { get; }

    bool TryJoin(int seatIndex, PlayerRole role);
    bool TryLeave(int seatIndex);
    bool TrySwitchRole(int seatIndex, PlayerRole newRole);
}
