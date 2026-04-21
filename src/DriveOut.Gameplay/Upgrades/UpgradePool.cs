using DriveOut.Core.Upgrades;

namespace DriveOut.Gameplay.Upgrades;

public sealed class UpgradePool
{
    // Drop weights per rarity: Common 60%, Uncommon 25%, Rare 10%, Legendary 5%
    private static readonly Dictionary<UpgradeRarity, int> RarityWeights = new()
    {
        { UpgradeRarity.Common, 60 },
        { UpgradeRarity.Uncommon, 25 },
        { UpgradeRarity.Rare, 10 },
        { UpgradeRarity.Legendary, 5 }
    };

    private readonly Dictionary<UpgradeRarity, List<IUpgrade>> _byRarity = new();
    private readonly Random _random;

    public int TotalCount => _byRarity.Values.Sum(l => l.Count);

    public UpgradePool(int? seed = null)
    {
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
        foreach (UpgradeRarity r in Enum.GetValues<UpgradeRarity>())
            _byRarity[r] = [];
    }

    public void Add(IUpgrade upgrade) => _byRarity[upgrade.Rarity].Add(upgrade);

    public IReadOnlyList<IUpgrade> OfferChoices(int count)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

        var available = _byRarity.Values.SelectMany(l => l).ToList();
        if (available.Count == 0) return [];

        var chosen = new List<IUpgrade>();
        var seen = new HashSet<UpgradeType>();

        for (int i = 0; i < count && available.Count > 0; i++)
        {
            var rarity = RollRarity(available);
            var pool = _byRarity[rarity].Where(u => !seen.Contains(u.Type)).ToList();

            if (pool.Count == 0)
            {
                pool = available.Where(u => !seen.Contains(u.Type)).ToList();
                if (pool.Count == 0) break;
            }

            var pick = pool[_random.Next(pool.Count)];
            chosen.Add(pick);
            seen.Add(pick.Type);
        }

        return chosen;
    }

    private UpgradeRarity RollRarity(List<IUpgrade> available)
    {
        var availableRarities = available
            .Select(u => u.Rarity)
            .Distinct()
            .ToHashSet();

        int totalWeight = RarityWeights
            .Where(kv => availableRarities.Contains(kv.Key))
            .Sum(kv => kv.Value);

        int roll = _random.Next(totalWeight);
        int cumulative = 0;

        foreach (var (rarity, weight) in RarityWeights)
        {
            if (!availableRarities.Contains(rarity)) continue;
            cumulative += weight;
            if (roll < cumulative) return rarity;
        }

        return availableRarities.First();
    }
}
