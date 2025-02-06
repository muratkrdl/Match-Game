public class PlayerReadyState : IPlayerState
{
    public void EnterState()
    {
        GameStateManager.Instance.SetCanPlayerInteract(true);
    }

    public void ExitState()
    {
        GameStateManager.Instance.SetCanPlayerInteract(false);
    }

    public void UpdateState()
    {

    }
}
