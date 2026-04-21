using DriveOut.Core.Feedback;
using DriveOut.Systems.Feedback;

namespace DriveOut.Tests.Feedback;

public sealed class FeedbackBusTests
{
    [Fact]
    public void Trigger_WithoutIntensity_UsesDefaultOne()
    {
        var bus = new FeedbackBus();
        (FeedbackEvent evt, float intensity) received = default;
        bus.OnFeedback += (e, i) => received = (e, i);

        bus.Trigger(FeedbackEvent.HazardHit);

        received.evt.Should().Be(FeedbackEvent.HazardHit);
        received.intensity.Should().Be(1f);
    }

    [Fact]
    public void Trigger_WithIntensity_PassesValue()
    {
        var bus = new FeedbackBus();
        float received = 0f;
        bus.OnFeedback += (_, i) => received = i;

        bus.Trigger(FeedbackEvent.PlayerDeath, 0.5f);

        received.Should().BeApproximately(0.5f, 0.001f);
    }

    [Fact]
    public void Trigger_NegativeIntensity_Throws()
    {
        var bus = new FeedbackBus();
        var act = () => bus.Trigger(FeedbackEvent.HazardHit, -0.1f);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Trigger_MultipleListeners_AllReceive()
    {
        var bus = new FeedbackBus();
        int count = 0;
        bus.OnFeedback += (_, _) => count++;
        bus.OnFeedback += (_, _) => count++;

        bus.Trigger(FeedbackEvent.UpgradeCollected);

        count.Should().Be(2);
    }

    [Fact]
    public void Trigger_NoListeners_DoesNotThrow()
    {
        var bus = new FeedbackBus();
        var act = () => bus.Trigger(FeedbackEvent.BossDefeated);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(FeedbackEvent.RunStarted)]
    [InlineData(FeedbackEvent.HazardHit)]
    [InlineData(FeedbackEvent.DamageTaken)]
    [InlineData(FeedbackEvent.UpgradeCollected)]
    [InlineData(FeedbackEvent.BossDefeated)]
    [InlineData(FeedbackEvent.SpeedBoostActive)]
    [InlineData(FeedbackEvent.PlayerDeath)]
    public void Trigger_AllEvents_FireCorrectly(FeedbackEvent evt)
    {
        var bus = new FeedbackBus();
        FeedbackEvent? received = null;
        bus.OnFeedback += (e, _) => received = e;

        bus.Trigger(evt);

        received.Should().Be(evt);
    }
}
