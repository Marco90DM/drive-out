using DriveOut.Core.Map;

namespace DriveOut.Tests.Map;

public sealed class TunnelSectionTests
{
    [Fact]
    public void TunnelSection_Record_StoresAllFields()
    {
        var section = new TunnelSection(TunnelSectionType.CurveLeft, 20f, 5f, 0.3f);

        section.Type.Should().Be(TunnelSectionType.CurveLeft);
        section.Length.Should().Be(20f);
        section.Width.Should().Be(5f);
        section.Difficulty.Should().Be(0.3f);
    }

    [Fact]
    public void TunnelSection_ValueEquality()
    {
        var a = new TunnelSection(TunnelSectionType.Straight, 25f, 6f, 0f);
        var b = new TunnelSection(TunnelSectionType.Straight, 25f, 6f, 0f);
        a.Should().Be(b);
    }

    [Theory]
    [InlineData(TunnelSectionType.Straight)]
    [InlineData(TunnelSectionType.CurveLeft)]
    [InlineData(TunnelSectionType.CurveRight)]
    [InlineData(TunnelSectionType.Narrowing)]
    [InlineData(TunnelSectionType.Wide)]
    public void AllSectionTypes_Representable(TunnelSectionType type)
    {
        var section = new TunnelSection(type, 10f, 4f, 0f);
        section.Type.Should().Be(type);
    }
}
