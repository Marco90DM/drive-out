using DriveOut.Core.Difficulty;
using DriveOut.Core.Events;
using DriveOut.Systems.Events;

namespace DriveOut.Tests.Events;

public sealed class EventBusTests
{
    [Fact]
    public void Subscribe_NullHandler_Throws()
    {
        var bus = new EventBus<int>();
        var act = () => bus.Subscribe(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Publish_DeliversToSubscriber()
    {
        var bus = new EventBus<int>();
        int received = 0;
        bus.Subscribe(v => received = v);
        bus.Publish(42);
        received.Should().Be(42);
    }

    [Fact]
    public void Publish_DeliversToAllSubscribers()
    {
        var bus = new EventBus<string>();
        var results = new List<string>();
        bus.Subscribe(s => results.Add("A:" + s));
        bus.Subscribe(s => results.Add("B:" + s));
        bus.Publish("x");
        results.Should().HaveCount(2);
    }

    [Fact]
    public void Subscribe_SameHandler_RegisteredOnce()
    {
        var bus = new EventBus<int>();
        int count = 0;
        Action<int> h = _ => count++;
        bus.Subscribe(h);
        bus.Subscribe(h);
        bus.Publish(1);
        count.Should().Be(1);
    }

    [Fact]
    public void Unsubscribe_RemovesHandler()
    {
        var bus = new EventBus<int>();
        int count = 0;
        Action<int> h = _ => count++;
        bus.Subscribe(h);
        bus.Unsubscribe(h);
        bus.Publish(1);
        count.Should().Be(0);
    }

    [Fact]
    public void Unsubscribe_NonRegistered_DoesNotThrow()
    {
        var bus = new EventBus<int>();
        var act = () => bus.Unsubscribe(_ => { });
        act.Should().NotThrow();
    }

    [Fact]
    public void Publish_HandlerUnsubscribesDuringSelf_DoesNotThrow()
    {
        var bus = new EventBus<int>();
        Action<int>? h = null;
        h = _ => bus.Unsubscribe(h!);
        bus.Subscribe(h);
        var act = () => bus.Publish(1);
        act.Should().NotThrow();
    }

    [Fact]
    public void Clear_RemovesAllHandlers()
    {
        var bus = new EventBus<int>();
        int count = 0;
        bus.Subscribe(_ => count++);
        bus.Subscribe(_ => count++);
        bus.Clear();
        bus.Publish(1);
        count.Should().Be(0);
        bus.HandlerCount.Should().Be(0);
    }

    [Fact]
    public void GameEvents_RunStartedEvent_RecordsLevel()
    {
        var bus = new EventBus<RunStartedEvent>();
        DifficultyLevel? received = null;
        bus.Subscribe(e => received = e.Difficulty);
        bus.Publish(new RunStartedEvent(DifficultyLevel.Hard));
        received.Should().Be(DifficultyLevel.Hard);
    }
}
