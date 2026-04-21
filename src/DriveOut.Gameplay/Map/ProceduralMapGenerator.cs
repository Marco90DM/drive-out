using DriveOut.Core.Map;

namespace DriveOut.Gameplay.Map;

public sealed class ProceduralMapGenerator
{
    // Section type weights at difficulty 0 → 1:
    //   Straight: 50 → 20   (fewer straights at high difficulty)
    //   Wide:     20 → 5    (fewer wide sections)
    //   CurveLeft: 10 → 20  (more curves)
    //   CurveRight: 10 → 20
    //   Narrowing: 10 → 35  (most dangerous at high difficulty)
    private static readonly (TunnelSectionType Type, float WeightAt0, float WeightAt1)[] WeightTable =
    [
        (TunnelSectionType.Straight,   50f, 20f),
        (TunnelSectionType.Wide,       20f,  5f),
        (TunnelSectionType.CurveLeft,  10f, 20f),
        (TunnelSectionType.CurveRight, 10f, 20f),
        (TunnelSectionType.Narrowing,  10f, 35f),
    ];

    private Random _random;
    private readonly float _difficultyRampRate;
    private int _sectionIndex;

    public int Seed { get; }
    public int SectionIndex => _sectionIndex;

    public ProceduralMapGenerator(int seed, float difficultyRampRate = 0.01f)
    {
        if (difficultyRampRate <= 0f) throw new ArgumentOutOfRangeException(nameof(difficultyRampRate));
        Seed = seed;
        _difficultyRampRate = difficultyRampRate;
        _random = new Random(seed);
    }

    public TunnelSection NextSection()
    {
        float difficulty = Math.Clamp(_sectionIndex * _difficultyRampRate, 0f, 1f);
        var type = RollType(difficulty);
        float length = RollLength(type, difficulty);
        float width = RollWidth(type, difficulty);
        _sectionIndex++;
        return new TunnelSection(type, length, width, difficulty);
    }

    public void Reset()
    {
        _sectionIndex = 0;
        _random = new Random(Seed);
    }

    private TunnelSectionType RollType(float difficulty)
    {
        float total = 0f;
        Span<float> weights = stackalloc float[WeightTable.Length];
        for (int i = 0; i < WeightTable.Length; i++)
        {
            var (_, w0, w1) = WeightTable[i];
            weights[i] = w0 + (w1 - w0) * difficulty;
            total += weights[i];
        }

        float roll = (float)_random.NextDouble() * total;
        float cumulative = 0f;
        for (int i = 0; i < WeightTable.Length; i++)
        {
            cumulative += weights[i];
            if (roll < cumulative) return WeightTable[i].Type;
        }
        return WeightTable[^1].Type;
    }

    private float RollLength(TunnelSectionType type, float difficulty) => type switch
    {
        TunnelSectionType.Wide => Lerp(30f, 25f, difficulty) + (float)(_random.NextDouble() * 10 - 5),
        TunnelSectionType.Straight => Lerp(25f, 20f, difficulty) + (float)(_random.NextDouble() * 10 - 5),
        TunnelSectionType.Narrowing => Lerp(20f, 10f, difficulty) + (float)(_random.NextDouble() * 6 - 3),
        _ => Lerp(20f, 15f, difficulty) + (float)(_random.NextDouble() * 8 - 4),
    };

    private static float RollWidth(TunnelSectionType type, float difficulty) => type switch
    {
        TunnelSectionType.Wide => 8f,
        TunnelSectionType.Straight => 6f,
        TunnelSectionType.CurveLeft or TunnelSectionType.CurveRight => 5f,
        TunnelSectionType.Narrowing => Lerp(4f, 2f, difficulty),
        _ => 6f,
    };

    private static float Lerp(float a, float b, float t) => a + (b - a) * t;
}
