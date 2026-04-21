namespace DriveOut.Core.Roles;

public sealed class PlayerSeat
{
    public int Index { get; }
    public PlayerRole Role { get; private set; }
    public bool IsOccupied { get; private set; }

    public PlayerSeat(int index) => Index = index;

    public void Join(PlayerRole role)
    {
        Role = role;
        IsOccupied = true;
    }

    public void Leave() => IsOccupied = false;

    public void ChangeRole(PlayerRole role) => Role = role;
}
