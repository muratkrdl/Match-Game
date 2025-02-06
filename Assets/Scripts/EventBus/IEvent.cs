public interface IEvent { /* */ }

public struct OnGameStateChangee : IEvent
{
    public IPlayerState state;
}
