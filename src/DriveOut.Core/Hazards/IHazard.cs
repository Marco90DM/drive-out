namespace DriveOut.Core.Hazards;

public interface IHazard
{
    HazardType Type { get; }
    HazardEffect Effect { get; }

    void OnHit(IHazardTarget target);
}
