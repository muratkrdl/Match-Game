using System.Collections.Generic;

public class EventBus<T> where T : IEvent
{
    private static readonly HashSet<IEventAction<T>> bindings = new();

    public static void Subscribe(EventAction<T> binding) => bindings.Add(binding);
    public static void UnSubscribe(EventAction<T> binding) => bindings.Remove(binding);

    public static void Publish(T eventToPublish)
    {
        foreach(var item in bindings)
        {
            item.OnEvent.Invoke(eventToPublish);
            item.OnEventNoArgs.Invoke();
        }
    }

    private static void Clear()
    {
        bindings.Clear();
    }
}
