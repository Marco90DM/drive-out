namespace DriveOut.Core;

public enum HazardType
{
    BananaPeel,
    EngineOil,
    Cardboard,
    Spikes,
    LightFlashing
}

public enum BossType
{
    GoblinVan,
    VentMonster
}

public enum RoleType
{
    Driver,
    Gps,
    Fighter,
    Repairer
}

public enum UpgradeType
{
    WindshieldFluid,
    MusicCd,
    HangingAirFreshener,
    BeerBottle,
    Brick
}

public sealed record GameConfig(
    int BossIntervalSeconds = 8,
    int BananaSteeringLockSeconds = 3,
    int MaxRunHistory = 20);
