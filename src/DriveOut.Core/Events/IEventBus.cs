namespace DriveOut.Core.Events;

public interface IEventBus<T>
{
    void Subscribe(Action<T> handler);
    void Unsubscribe(Action<T> handler);
    void Publish(T payload);
}
