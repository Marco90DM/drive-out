using DriveOut.Core;

namespace DriveOut.Infrastructure;

public sealed record RunSummary(
    DateTimeOffset CompletedAt,
    TimeSpan Duration,
    int DamageTaken,
    int Seed);

public sealed class RunHistoryRepository(GameConfig config)
{
    private readonly Queue<RunSummary> _runs = new();

    public void Add(RunSummary run)
    {
        _runs.Enqueue(run);
        while (_runs.Count > config.MaxRunHistory)
        {
            _runs.Dequeue();
        }
    }

    public IReadOnlyCollection<RunSummary> GetLastRuns()
    {
        return _runs.ToArray();
    }
}
