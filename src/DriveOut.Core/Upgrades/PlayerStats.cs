namespace DriveOut.Core.Upgrades;

public sealed class PlayerStats
{
    public float MaxSpeedBonus { get; set; }
    public float ArmorBonus { get; set; }
    public float ScoreMultiplier { get; set; } = 1f;
    public int HazardShieldCharges { get; set; }
    public float SteerSpeedBonus { get; set; }

    public void Reset()
    {
        MaxSpeedBonus = 0f;
        ArmorBonus = 0f;
        ScoreMultiplier = 1f;
        HazardShieldCharges = 0;
        SteerSpeedBonus = 0f;
    }
}
