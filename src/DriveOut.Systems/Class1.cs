using DriveOut.Core;

namespace DriveOut.Systems;

public sealed class BossEncounterScheduler(GameConfig config)
{
    public bool ShouldSpawnBoss(float runTimeSeconds)
    {
        if (runTimeSeconds <= 0)
        {
            return false;
        }

        var interval = Math.Max(1, config.BossIntervalSeconds);
        return Math.Abs(runTimeSeconds % interval) < 0.001f;
    }
}

public sealed class UpgradeOfferService
{
    private static readonly UpgradeType[] Pool = Enum.GetValues<UpgradeType>();

    public IReadOnlyList<UpgradeType> BuildOffer(int offerSize, int seed)
    {
        if (offerSize <= 0)
        {
            return [];
        }

        var random = new Random(seed);
        var offer = new List<UpgradeType>(Math.Min(offerSize, Pool.Length));
        var workingPool = Pool.ToList();

        while (offer.Count < offerSize && workingPool.Count > 0)
        {
            var index = random.Next(workingPool.Count);
            offer.Add(workingPool[index]);
            workingPool.RemoveAt(index);
        }

        return offer;
    }
}
