using DriveOut.Core.Upgrades;

namespace DriveOut.Gameplay.Upgrades;

public sealed class UpgradeManager
{
    private readonly List<IUpgrade> _acquired = [];

    public PlayerStats Stats { get; } = new();
    public IReadOnlyList<IUpgrade> Acquired => _acquired;

    public event Action<IUpgrade>? OnUpgradeApplied;

    public void Apply(IUpgrade upgrade)
    {
        upgrade.Apply(Stats);
        _acquired.Add(upgrade);
        OnUpgradeApplied?.Invoke(upgrade);
    }

    public bool TryConsumeShieldCharge()
    {
        if (Stats.HazardShieldCharges <= 0) return false;
        Stats.HazardShieldCharges--;
        return true;
    }

    public void Reset()
    {
        _acquired.Clear();
        Stats.Reset();
    }
}
