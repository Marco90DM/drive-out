namespace DriveOut.Core.Map;

public sealed record TunnelSection(
    TunnelSectionType Type,
    float Length,
    float Width,
    float Difficulty);
