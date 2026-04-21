using DriveOut.Core.Events;

namespace DriveOut.Systems.Events;

public sealed class EventBus<T> : IEventBus<T>
{
    private readonly List<Action<T>> _handlers = [];

    public int HandlerCount => _handlers.Count;

    public void Subscribe(Action<T> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        if (!_handlers.Contains(handler))
            _handlers.Add(handler);
    }

    public void Unsubscribe(Action<T> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        _handlers.Remove(handler);
    }

    public void Publish(T payload)
    {
        // Copy to avoid issues if a handler modifies the subscription list
        foreach (var handler in _handlers.ToArray())
            handler(payload);
    }

    public void Clear() => _handlers.Clear();
}
