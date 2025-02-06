using System;

public interface IEventAction<T>
{
    public Action<T> OnEvent { get; set; }
    public Action OnEventNoArgs { get; set; }
}

public class EventAction<T> : IEventAction<T> where T : IEvent
{
    Action<T> onEvent = _ => { };
    Action onEventNoArgs = () => { };

    Action<T> IEventAction<T>.OnEvent 
    {
        get => onEvent;
        set => onEvent = value;
    }

    Action IEventAction<T>.OnEventNoArgs 
    {
        get => onEventNoArgs;
        set => onEventNoArgs = value;
    }

    public EventAction(Action<T> onEvent)
    {
        this.onEvent = onEvent;
    }
    public EventAction(Action onEventNoArgs)
    {
        this.onEventNoArgs = onEventNoArgs;
    }

    public void Add(Action onEvent) => onEventNoArgs += onEvent;
    public void Remove(Action onEvent) => onEventNoArgs -= onEvent;
    
    public void Add(Action<T> onEvent) => this.onEvent += onEvent;
    public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;
}
