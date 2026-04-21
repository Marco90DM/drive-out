namespace DriveOut.Core.Progression;

public abstract class UnlockCondition
{
    public abstract bool IsMet(MetaProgress progress);
}

public sealed class RunCountCondition : UnlockCondition
{
    private readonly int _required;
    public RunCountCondition(int required)
    {
        if (required <= 0) throw new ArgumentOutOfRangeException(nameof(required));
        _required = required;
    }
    public override bool IsMet(MetaProgress progress) => progress.TotalRuns >= _required;
}

public sealed class MaxDistanceCondition : UnlockCondition
{
    private readonly float _required;
    public MaxDistanceCondition(float required)
    {
        if (required <= 0f) throw new ArgumentOutOfRangeException(nameof(required));
        _required = required;
    }
    public override bool IsMet(MetaProgress progress) => progress.AllTimeMaxDistance >= _required;
}

public sealed class TotalDistanceCondition : UnlockCondition
{
    private readonly float _required;
    public TotalDistanceCondition(float required)
    {
        if (required <= 0f) throw new ArgumentOutOfRangeException(nameof(required));
        _required = required;
    }
    public override bool IsMet(MetaProgress progress) => progress.AllTimeTotalDistance >= _required;
}
